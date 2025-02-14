using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Identity.Enums;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Users;
using MLMS.Infrastructure.Identity.Models;
using MLMS.Infrastructure.Persistence;
using Sieve.Models;
using Sieve.Services;

namespace MLMS.Infrastructure.Identity;

public class UserRepository(
    LmsDbContext context,
    UserManager<ApplicationUser> userManager,
    ISieveProcessor sieveProcessor) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await userManager.Users
            .Where(u => u.Id == id)
            .Include(u => u.Major)
            .Include(u => u.Department)
            .Include(u => u.Role)
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync();

        return user?.ToDomain();
    }

    public async Task<int> CreateAsync(User user, string password)
    {
        var userToCreate = user.ToIdentityUser();

        userToCreate.UserName = Guid.NewGuid().ToString();
        
        var role = await context.Roles.FirstAsync(r => r.Name == user.Role.ToString());
        
        userToCreate.RoleId = role.Id;
        
        var creationResult = await userManager.CreateAsync(userToCreate, password);

        if (!creationResult.Succeeded)
        {
            throw new Exception(string.Join(",", creationResult.Errors.Select(e => e.Description)));
        }

        return userToCreate.Id;
    }

    public async Task<bool> ExistsByWorkIdAsync(string workId)
    {
        return await userManager.Users.AnyAsync(u => u.WorkId == workId);
    }

    public async Task<User?> GetByWorkIdAsync(string workId)
    {
        var identityUser = await userManager.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.WorkId == workId);

        return identityUser?.ToDomain();
    }

    public async Task UpdateAsync(User user)
    {
        var userToUpdate = await context.Users.FindAsync(user.Id);

        if (userToUpdate is null)
        {
            return;
        }
        
        userToUpdate.MapForUpdate(user.ToIdentityUser());

        await userManager.UpdateAsync(userToUpdate);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<PaginatedList<User>> GetAsync(SieveModel sieveModel)
    {
        var query = context.Users
            .IgnoreQueryFilters()
            .Include(u => u.Role)
            .Include(u => u.Major)
            .Include(u => u.Department)
            .Include(u => u.ProfilePicture)
            .AsQueryable();

        var totalItems = await sieveProcessor.Apply(sieveModel, query, applyPagination: false).CountAsync();
        
        query = sieveProcessor.Apply(sieveModel, query);

        var result = await query.AsNoTracking()
            .ToListAsync();

        return new PaginatedList<User>
        {
            Items = result.Select(u => u.ToDomain()).ToList(),
            Metadata = new PaginationMetadata
            {
                TotalItems = totalItems,
                PageSize = sieveModel.PageSize!.Value,
                Page = sieveModel.Page!.Value
            }
        };
    }

    public async Task<List<User>> GetByMajorsAsync(List<int> majors)
    {
        var query = from u in context.Users.Include(u => u.Role)
            join m in majors on u.MajorId equals m
            select u;

        return (await query.AsNoTracking()
                .ToListAsync())
            .Select(u => u.ToDomain())
            .ToList();
    }

    public async Task<bool> ExistsByIdAsync(int userId)
    {
        return await context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<bool> IsSubAdminAsync(int userId)
    {
        return await context.Users.AnyAsync(u => u.Id == userId && u.Role.Name == UserRole.SubAdmin.ToString());
    }

    public async Task ChangeUserStatusAsync(int id, bool isActive)
    {
        await context.Users.Where(u => u.Id == id)
            .ExecuteUpdateAsync(spc => spc.SetProperty(n => n.IsActive, isActive));
    }

    public async Task UpdateProfilePictureAsync(int id, Guid imageId)
    {
        var user = await context.Users.FindAsync(id);

        if (user is null)
        {
            return;
        }

        user.ProfilePictureId = imageId;

        await context.SaveChangesAsync();
    }
}