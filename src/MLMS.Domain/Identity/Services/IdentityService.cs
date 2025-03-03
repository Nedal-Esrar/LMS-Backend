using ErrorOr;
using Microsoft.Extensions.Options;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Exceptions;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Departments;
using MLMS.Domain.Email;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Identity.Models;
using MLMS.Domain.Identity.Validators;
using MLMS.Domain.Majors;
using MLMS.Domain.Users;

namespace MLMS.Domain.Identity.Services;

public class IdentityService(
    IUserService userService,
    IAuthService authService,
    ITokenGenerator tokenGenerator,
    IMajorService majorService,
    IDepartmentService departmentService,
    IPasswordGenerationService passwordGenerator,
    IDbTransactionProvider dbTransactionProvider,
    IEmailService emailService,
    IRefreshTokenRepository refreshTokenRepository,
    IUserContext userContext,
    IOptions<ClientOptions> clientOptions,
    ICourseAssignmentService courseAssignmentService) : IIdentityService
{
    private readonly LoginValidator _loginValidator = new();
    private readonly RegisterValidator _registerValidator = new();
    private readonly ClientOptions _clientOptions = clientOptions.Value;
    
    public async Task<ErrorOr<UserTokens>> AuthenticateAsync(LoginCredentials loginCredentials)
    {
        var validationResult = await _loginValidator.ValidateAsync(loginCredentials);

        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }
        
        var user = await authService.AuthenticateAsync(loginCredentials);

        if (user is null)
        {
            return IdentityErrors.InvalidCredentials;
        }

        if (!user.IsActive)
        {
            return IdentityErrors.UserNotActive;
        }
        
        var accessToken = tokenGenerator.GenerateAccessToken(user);
        var refreshToken = tokenGenerator.GenerateRefreshToken(user.Id);
        
        await refreshTokenRepository.CreateAsync(refreshToken);

        return new UserTokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<ErrorOr<None>> RegisterAsync(User user)
    {
        var validationResult = await _registerValidator.ValidateAsync(user);

        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }

        var userExistenceResult = await userService.CheckExistenceByWorkIdAsync(user.WorkId);

        if (userExistenceResult.IsError)
        {
            return userExistenceResult.Errors;
        }

        var userWithSameWorkIdExists = userExistenceResult.Value;
        
        if (userWithSameWorkIdExists)
        {
            return UserErrors.WorkIdExists;
        }

        var departmentExistenceResult = await departmentService.CheckExistenceAsync(user.DepartmentId!.Value);

        if (departmentExistenceResult.IsError)
        {
            return departmentExistenceResult.Errors;
        }

        var majorExistenceResult =
            await majorService.CheckExistenceAsync(user.DepartmentId!.Value, user.MajorId!.Value);

        if (majorExistenceResult.IsError)
        {
            return majorExistenceResult.Errors;
        }
        
        var passwordResult = passwordGenerator.GenerateStrongPassword(length: 8);
        
        if (passwordResult.IsError)
        {
            return passwordResult.Errors;
        }

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            user.IsActive = true;
            var userCreationResult = await userService.CreateAsync(user, passwordResult.Value);

            if (userCreationResult.IsError)
            {
                throw new UnoccasionalErrorException(userCreationResult.Errors);
            }

            var userId = userCreationResult.Value;

            var assignmentsCreationResult = await courseAssignmentService.CreateAssignmentsAsync(userId, user.MajorId.Value);

            if (assignmentsCreationResult.IsError)
            {
                throw new UnoccasionalErrorException(assignmentsCreationResult.Errors);
            }

            await emailService.SendAsync(new EmailRequest
            {
                ToEmails = [user.Email],
                Subject = "Your LMS account has been created successfully",
                BodyHtml = EmailUtils.GetRegistrationEmailBody($"{user.FirstName} {user.MiddleName} {user.LastName}", user.WorkId, passwordResult.Value, $"{_clientOptions.BaseUrl}/{_clientOptions.LoginRoute}")
            });
        });

        return None.Value;
    }

    public async Task<ErrorOr<UserTokens>> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenModel = await refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (refreshTokenModel is null)
        {
            return IdentityErrors.InvalidRefreshToken;
        }
        
        if (refreshTokenModel.Expiration < DateTime.UtcNow)
        {
            await refreshTokenRepository.DeleteAsync(refreshTokenModel.Id);
            
            return IdentityErrors.InvalidRefreshToken;
        }

        var userRetrievalResult = await userService.GetByIdAsync(refreshTokenModel.UserId);

        if (userRetrievalResult.IsError)
        {
            return userRetrievalResult.Errors;
        }

        var user = userRetrievalResult.Value;

        var accessToken = tokenGenerator.GenerateAccessToken(user!);
        var newRefreshToken = tokenGenerator.GenerateRefreshToken(user!.Id);

        refreshTokenModel.Expiration = newRefreshToken.Expiration;
        refreshTokenModel.Token = newRefreshToken.Token;
        
        await refreshTokenRepository.UpdateAsync(refreshTokenModel);

        return new UserTokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenModel
        };
    }

    public async Task<ErrorOr<None>> RevokeRefreshTokenAsync(string refreshToken)
    {
        if (!await refreshTokenRepository.ExistsByTokenAsync(refreshToken))
        {
            return IdentityErrors.InvalidRefreshToken;
        }
        
        await refreshTokenRepository.DeleteAsync(refreshToken);

        return None.Value;
    }

    public async Task<ErrorOr<None>> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        if (!await authService.IsOwnPasswordAsync(userContext.Id, currentPassword))
        {
            return IdentityErrors.WrongCurrentPassword;
        }

        if (!PasswordUtils.IsStrongPassword(newPassword))
        {
            return IdentityErrors.WeakPassword;
        }
        
        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            await authService.ChangePasswordAsync(userContext.Id, currentPassword, newPassword);

            await emailService.SendAsync(new EmailRequest
            {
                ToEmails = [userContext.Email],
                Subject = "Your password has been changed",
                BodyHtml = EmailUtils.GetPasswordChangedEmailBody($"{userContext.Name}")
            });
        });

        return None.Value;
    }

    public async Task<ErrorOr<None>> ForgotPasswordAsync(string workId)
    {
        var userRetrievalResult = await userService.GetByWorkIdAsync(workId);

        if (userRetrievalResult.IsError)
        {
            return userRetrievalResult.Errors;
        }

        var user = userRetrievalResult.Value;
        
        var token = await authService.GenerateResetPasswordTokenAsync(user.Id);
        
        await emailService.SendAsync(new EmailRequest
        {
            ToEmails = [user.Email],
            Subject = "Reset your Makassed LMS password",
            BodyHtml = EmailUtils.GetResetPasswordEmailBody($"{user.FirstName} {user.MiddleName} {user.LastName}", $"{_clientOptions.BaseUrl}/{_clientOptions.ResetPasswordRoute}?token={token}&workId={user.WorkId}")
        });

        return None.Value;
    }

    public async Task<ErrorOr<None>> ResetPasswordAsync(string workId, string newPassword, string token)
    {
        var userRetrievalResult = await userService.GetByWorkIdAsync(workId);

        if (userRetrievalResult.IsError)
        {
            return userRetrievalResult.Errors;
        }

        var user = userRetrievalResult.Value;

        if (!PasswordUtils.IsStrongPassword(newPassword))
        {
            return IdentityErrors.WeakPassword;
        }

        if (!await authService.ValidateResetPasswordToken(user.Id, token))
        {
            return IdentityErrors.InvalidResetPasswordToken;
        }
        
        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            await authService.ResetPasswordAsync(user.Id, token, newPassword);

            await emailService.SendAsync(new EmailRequest
            {
                ToEmails = [user.Email],
                Subject = "Your password has been changed",
                BodyHtml = EmailUtils.GetPasswordChangedEmailBody($"{user.FirstName} {user.MiddleName} {user.LastName}")
            });
        });

        return None.Value;
    }

    public async Task<ErrorOr<bool>> ValidateResetPasswordTokenAsync(string workId, string token)
    {
        var userRetrievalResult = await userService.GetByWorkIdAsync(workId);

        if (userRetrievalResult.IsError)
        {
            return userRetrievalResult.Errors;
        }

        var user = userRetrievalResult.Value;
        
        return await authService.ValidateResetPasswordToken(user.Id, token);
    }
}