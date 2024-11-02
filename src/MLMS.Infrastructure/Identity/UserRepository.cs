using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Entities;
using MLMS.Domain.Enums;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Infrastructure.Common;
using MLMS.Infrastructure.Identity.Models;

namespace MLMS.Infrastructure.Identity;

public class UserRepository(
    LmsDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<int>> roleManager) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await userManager.Users
            .Include(u => u.Major)
            .Include(u => u.Department)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        user.Roles = (await userManager.GetRolesAsync(user)).Select(Enum.Parse<UserRole>)
            .ToList();

        return user?.ToDomain();
    }

    public async Task CreateAsync(User user, string password)
    {
        var userToCreate = user.ToIdentityUser();
        
        var creationResult = await userManager.CreateAsync(userToCreate, password);

        if (!creationResult.Succeeded)
        {
            throw new Exception(string.Join(",", creationResult.Errors.Select(e => e.Description)));
        }

        var addToRolesResult = await userManager.AddToRolesAsync(userToCreate, user.Roles.Select(r => r.ToString()));
        
        if (!addToRolesResult.Succeeded)
        {
            throw new Exception(string.Join(",", addToRolesResult.Errors.Select(e => e.Description)));
        }
    }

    public async Task<bool> ExistsByWorkIdAsync(string workId)
    {
        return await userManager.Users.AnyAsync(u => u.WorkId == workId);
    }

    public async Task<User?> GetByWorkIdAsync(string workId)
    {
        var identityUser = await userManager.Users.FirstOrDefaultAsync(u => u.WorkId == workId);

        return identityUser?.ToDomain();
    }
}