using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Departments;
using MLMS.Infrastructure.Common;

namespace MLMS.Infrastructure.Departments;

public class DepartmentRepository(LmsDbContext context) : IDepartmentRepository
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

    public async Task<List<Department>> GetAsync()
    {
        return await context.Departments.ToListAsync();
    }

    public async Task<bool> ExistsByNameAsync(string departmentName)
    {
        return await context.Departments.AnyAsync(d => d.Name == departmentName);
    }
}