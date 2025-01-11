using ErrorOr;
using Microsoft.Extensions.Options;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Departments;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Identity.Validators;
using MLMS.Domain.Majors;
using MLMS.Domain.Users;
using MLMS.Domain.UsersCourses;

namespace MLMS.Domain.Identity;

public class IdentityService(
    IUserRepository userRepository,
    IAuthService authService,
    ITokenGenerator tokenGenerator,
    IMajorRepository majorRepository,
    IDepartmentRepository departmentRepository,
    IPasswordGenerationService passwordGenerator,
    IDbTransactionProvider dbTransactionProvider,
    IEmailService emailService,
    IRefreshTokenRepository refreshTokenRepository,
    IUserContext userContext,
    IOptions<ClientOptions> clientOptions,
    IUserCourseRepository userCourseRepository,
    ICourseAssignmentRepository courseAssignmentRepository) : IIdentityService
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
        
        if (await userRepository.ExistsByWorkIdAsync(user.WorkId))
        {
            return UserErrors.WorkIdExists;
        }

        if (!await departmentRepository.ExistsAsync(user.DepartmentId!.Value))
        {
            return DepartmentErrors.NotFound;
        }
        
        if (!await majorRepository.ExistsAsync(user.DepartmentId.Value, user.MajorId!.Value))
        {
            return MajorErrors.NotFound;
        }
        
        var passwordResult = passwordGenerator.GenerateStrongPassword(length: 8);
        
        if (passwordResult.IsError)
        {
            return passwordResult.Errors;
        }

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            var userId = await userRepository.CreateAsync(user, passwordResult.Value);

            var courseAssignments = 
                await courseAssignmentRepository.GetByMajorIdAsync(user.MajorId.Value);

            var userCourseEntities = courseAssignments.Select(courseAssignment => 
                new UserCourse
                {
                    UserId = userId, 
                    CourseId = courseAssignment.CourseId, 
                    Status = UserCourseStatus.NotStarted
                }).ToList();

            await userCourseRepository.CreateAsync(userCourseEntities);

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

        var user = await userRepository.GetByIdAsync(refreshTokenModel.UserId);

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
        if (!await authService.IsOwnPasswordAsync(userContext.Id!.Value, currentPassword))
        {
            return IdentityErrors.WrongCurrentPassword;
        }

        if (!Utilities.IsStrongPassword(newPassword))
        {
            return IdentityErrors.WeakPassword;
        }
        
        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            await authService.ChangePasswordAsync(userContext.Id.Value, currentPassword, newPassword);

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
        var user = await userRepository.GetByWorkIdAsync(workId);

        if (user is null)
        {
            return UserErrors.NotFound;
        }
        
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
        var user = await userRepository.GetByWorkIdAsync(workId);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        if (!Utilities.IsStrongPassword(newPassword))
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
        var user = await userRepository.GetByWorkIdAsync(workId);

        if (user is null)
        {
            return UserErrors.NotFound;
        }
        
        return await authService.ValidateResetPasswordToken(user.Id, token);
    }
}