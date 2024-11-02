using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Majors;
using MLMS.Infrastructure.Common;

namespace MLMS.Infrastructure.Majors;

public class MajorRepository(LmsDbContext context) : IMajorRepository
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
        var major = await GetByIdAsync(departmentId, id);

        if (major is null)
        {
            return;
        }

        context.Majors.Remove(major);

        await context.SaveChangesAsync();
    }

    public async Task<Major?> GetByIdAsync(int departmentId, int id)
    {
        return await context.Majors.FirstOrDefaultAsync(m => m.DepartmentId == departmentId && m.Id == id);
    }

    public async Task<List<Major>> GetByDepartmentAsync(int departmentId)
    {
        return await context.Majors.Where(m => m.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameAsync(int majorDepartmentId, string majorName)
    {
        return await context.Majors.AnyAsync(m => m.DepartmentId == majorDepartmentId && m.Name == majorName);
    }
}