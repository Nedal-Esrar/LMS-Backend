using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.Options;
using MLMS.Application.Common;
using MLMS.Application.Departments;
using MLMS.Application.Majors;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Departments;
using MLMS.Domain.Entities;
using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Majors;
using MLMS.Domain.Models;

namespace MLMS.Application.Identity;

public class IdentityService(
    IValidator<LoginCredentials> loginValidator,
    IValidator<User> userValidator,
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
    IOptions<ClientOptions> clientOptions) : IIdentityService
{
    private readonly ClientOptions _clientOptions = clientOptions.Value;
    
    public async Task<ErrorOr<UserTokens>> AuthenticateAsync(LoginCredentials loginCredentials)
    {
        var validationResult = await loginValidator.ValidateAsync(loginCredentials);

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
        var validationResult = await userValidator.ValidateAsync(user);

        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }
        
        if (await userRepository.ExistsByWorkIdAsync(user.WorkId))
        {
            return IdentityErrors.WorkIdExists;
        }

        if (!await departmentRepository.ExistsAsync(user.DepartmentId))
        {
            return DepartmentErrors.NotFound;
        }
        
        if (!await majorRepository.ExistsAsync(user.DepartmentId, user.MajorId))
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
            await userRepository.CreateAsync(user, passwordResult.Value);

            await emailService.SendAsync(new EmailRequest
            {
                ToEmails = [user.Email],
                Subject = "Your LMS account has been created successfully",
                Body = $"Your account has been created successfully.\n Login with your work ID: {user.WorkId} and password: {passwordResult.Value}."
            });
        });

        return None.Value;
    }

    public async Task<ErrorOr<User>> GetUserAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return IdentityErrors.UserNotFound;
        }

        return user;
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
        if (!await authService.IsOwnPasswordAsync(userContext.Id.Value, currentPassword))
        {
            return IdentityErrors.WrongCurrentPassword;
        }

        if (!Utilities.IsStrongPassword(newPassword))
        {
            return IdentityErrors.WeakPassword;
        }
        
        await authService.ChangePasswordAsync(userContext.Id.Value, currentPassword, newPassword);

        return None.Value;
    }

    public async Task<ErrorOr<None>> ForgotPasswordAsync(string workId)
    {
        var user = await userRepository.GetByWorkIdAsync(workId);

        if (user is null)
        {
            return IdentityErrors.UserNotFound;
        }
        
        var token = await authService.GenerateResetPasswordTokenAsync(user.Id);
        
        await emailService.SendAsync(new EmailRequest
        {
            ToEmails = [user.Email],
            Subject = "Reset your Makassed LMS password",
            Body = $"Please click the link below to reset your password.\n {_clientOptions.BaseUrl}/{_clientOptions.ResetPasswordRoute}?token={token}&workId={user.WorkId}"
        });

        return None.Value;
    }

    public async Task<ErrorOr<None>> ResetPasswordAsync(string workId, string newPassword, string token)
    {
        var user = await userRepository.GetByWorkIdAsync(workId);

        if (user is null)
        {
            return IdentityErrors.UserNotFound;
        }

        if (!Utilities.IsStrongPassword(newPassword))
        {
            return IdentityErrors.WeakPassword;
        }
        
        if (!await authService.ResetPasswordAsync(user.Id, token, newPassword))
        {
            return IdentityErrors.InvalidResetPasswordToken;
        }

        return None.Value;
    }
}