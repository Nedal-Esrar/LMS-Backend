using ErrorOr;
using MLMS.Domain.Entities;
using MLMS.Domain.Users;

namespace MLMS.Domain.Identity.Interfaces;

public interface IIdentityService
{
    Task<ErrorOr<UserTokens>> AuthenticateAsync(LoginCredentials loginCredentials);
    
    Task<ErrorOr<None>> RegisterAsync(User user);
    
    Task<ErrorOr<User>> GetUserAsync(int id);
    
    Task<ErrorOr<UserTokens>> RefreshTokenAsync(string refreshToken);
    
    Task<ErrorOr<None>> RevokeRefreshTokenAsync(string refreshToken);
    
    Task<ErrorOr<None>> ChangePasswordAsync(string currentPassword, string newPassword);
    
    Task<ErrorOr<None>> ForgotPasswordAsync(string workId);
    
    Task<ErrorOr<None>> ResetPasswordAsync(string workId, string newPassword, string token);
}