using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Entities;
using MLMS.Domain.Enums;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Models;
using MLMS.Infrastructure.Identity.Models;

namespace MLMS.Infrastructure.Identity;

public class AuthService(UserManager<ApplicationUser> userManager) : IAuthService
{
    public async Task<User?> AuthenticateAsync(LoginCredentials loginCredentials)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.WorkId == loginCredentials.WorkId);

        if (user is null)
        {
            return null;
        }

        if (!await userManager.CheckPasswordAsync(user, loginCredentials.Password))
        {
            return null;
        }

        user.Roles = (await userManager.GetRolesAsync(user)).Select(Enum.Parse<UserRole>).ToList();

        return user.ToDomain();
    }

    public async Task<bool> IsOwnPasswordAsync(int userId, string password)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        return await userManager.CheckPasswordAsync(user!, password);
    }

    public async Task ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        await userManager.ChangePasswordAsync(user!, currentPassword, newPassword);
    }

    public async Task<string> GenerateResetPasswordTokenAsync(int userId)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        return await userManager.GeneratePasswordResetTokenAsync(user!);
    }

    public async Task<bool> ResetPasswordAsync(int userId, string token, string newPassword)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

        var resetPasswordResult = await userManager.ResetPasswordAsync(user, token, newPassword);

        return resetPasswordResult.Succeeded;
    }
}