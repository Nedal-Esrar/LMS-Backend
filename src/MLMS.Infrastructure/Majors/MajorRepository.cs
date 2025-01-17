using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Majors;
using MLMS.Infrastructure.Persistence;
using Sieve.Models;
using Sieve.Services;

namespace MLMS.Infrastructure.Majors;

public class MajorRepository(
    LmsDbContext context,
    IDbTransactionProvider dbTransactionProvider,
    ISieveProcessor sieveProcessor) : IMajorRepository
{
    public async Task<Major> CreateAsync(Major major)
    {
        context.Majors.Add(major);
        
        await context.SaveChangesAsync();

        return major;
    }

    public async Task<bool> ExistsAsync(int departmentId, int id)
    {
        return await context.Majors.AnyAsync(m => m.DepartmentId == departmentId && m.Id == id);
    }

    public async Task DeleteAsync(int departmentId, int id)
    {
        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            var major = await GetByIdAsync(departmentId, id);

            if (major is null)
            {
                return;
            }

            context.Majors.Remove(major);
        
            var users = await context.Users.Where(u => u.MajorId == id).ToListAsync();
        
            users.ForEach(u => u.MajorId = null);

            await context.SaveChangesAsync();
        });
    }

    public async Task<Major?> GetByIdAsync(int departmentId, int id)
    {
        return await context.Majors.FirstOrDefaultAsync(m => m.DepartmentId == departmentId && m.Id == id);
    }

    public async Task<PaginatedList<Major>> GetByDepartmentAsync(int departmentId, SieveModel sieveModel)
    {
        var query = context.Majors.Where(m => m.DepartmentId == departmentId)
            .AsQueryable();

        var totalItems = await sieveProcessor.Apply(sieveModel, query, applyPagination: false).CountAsync();

        query = sieveProcessor.Apply(sieveModel, query);

        var result = await query.AsNoTracking()
            .ToListAsync();

        return new PaginatedList<Major>
        {
            Items = result,
            Metadata = new PaginationMetadata
            {
                Page = sieveModel.Page!.Value,
                PageSize = sieveModel.PageSize!.Value,
                TotalItems = totalItems
            }
        };
    }

    public async Task<bool> ExistsByNameAsync(int majorDepartmentId, string majorName)
    {
        return await context.Majors.AnyAsync(m => m.DepartmentId == majorDepartmentId && m.Name == majorName);
    }

    public async Task UpdateAsync(Major major)
    {
        context.Majors.Update(major);
        
        await context.SaveChangesAsync();
    }
}