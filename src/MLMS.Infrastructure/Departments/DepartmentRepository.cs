using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Departments;
using MLMS.Infrastructure.Common;
using Sieve.Models;
using Sieve.Services;

namespace MLMS.Infrastructure.Departments;

public class DepartmentRepository(
    LmsDbContext context,
    ISieveProcessor sieveProcessor) : IDepartmentRepository
{
    public async Task<Department> CreateAsync(Department department)
    {
        context.Departments.Add(department);
        
        await context.SaveChangesAsync();

        return department;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await context.Departments.AnyAsync(d => d.Id == id);
    }

    public async Task DeleteAsync(int id)
    {
        var department = await context.Departments.FindAsync(id);

        if (department is null)
        {
            return;
        }
        
        context.Departments.Remove(department);
        
        await context.SaveChangesAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await context.Departments.FindAsync(id);
    }

    public async Task<PaginatedList<Department>> GetAsync(SieveModel sieveModel)
    {
        var query = context.Departments.AsQueryable();

        var totalItems = await sieveProcessor.Apply(sieveModel, query, applyPagination: false).CountAsync();

        query = sieveProcessor.Apply(sieveModel, query);

        var result = await query.AsNoTracking()
            .ToListAsync();

        return new PaginatedList<Department>
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

    public async Task<bool> ExistsByNameAsync(string departmentName)
    {
        return await context.Departments.AnyAsync(d => d.Name == departmentName);
    }

    public Task UpdateAsync(Department department)
    {
        context.Departments.Update(department);
        
        return context.SaveChangesAsync();
    }
}