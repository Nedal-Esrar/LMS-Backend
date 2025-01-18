using MLMS.Domain.Identity.Models;
using MLMS.Domain.Users;

namespace MLMS.Domain.Identity.Interfaces;

public interface IAuthService
{
    Task<User?> AuthenticateAsync(LoginCredentials loginCredentials);

    Task<bool> IsOwnPasswordAsync(int userId, string password);
    
    Task ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    
    Task<string> GenerateResetPasswordTokenAsync(int userId);
    
    Task<bool> ResetPasswordAsync(int userId, string token, string newPassword);

    Task<bool> ValidateResetPasswordToken(int userId, string token);
}