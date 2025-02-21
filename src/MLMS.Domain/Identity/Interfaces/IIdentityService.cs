using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Identity.Models;
using MLMS.Domain.Users;

namespace MLMS.Domain.Identity.Interfaces;

public interface IIdentityService
{
    Task<ErrorOr<UserTokens>> AuthenticateAsync(LoginCredentials loginCredentials);
    
    Task<ErrorOr<None>> RegisterAsync(User user);
    
    Task<ErrorOr<UserTokens>> RefreshTokenAsync(string refreshToken);
    
    Task<ErrorOr<None>> RevokeRefreshTokenAsync(string refreshToken);
    
    Task<ErrorOr<None>> ChangePasswordAsync(string currentPassword, string newPassword);
    
    Task<ErrorOr<None>> ForgotPasswordAsync(string workId);
    
    Task<ErrorOr<None>> ResetPasswordAsync(string workId, string newPassword, string token);
    
    Task<ErrorOr<bool>> ValidateResetPasswordTokenAsync(string workId, string token);
}