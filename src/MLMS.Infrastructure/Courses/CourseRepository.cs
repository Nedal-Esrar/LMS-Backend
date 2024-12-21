using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Courses;
using MLMS.Infrastructure.Common;
using Sieve.Models;

namespace MLMS.Infrastructure.Courses;

public class CourseRepository(LmsDbContext context) : ICourseRepository
{
    public async Task<Course> CreateAsync(Course course)
    {
        context.Courses.Add(course);
        
        await context.SaveChangesAsync();

        return course;
    }

    public async Task<bool> ExistsByNameAsync(string courseName)
    {
        return await context.Courses.AnyAsync(c => c.Name == courseName);
    }

    public Task<Course?> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsByIdAsync(long id)
    {
        return await context.Courses.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> IsCreatedByUserAsync(long id, int userId)
    {
        return await context.Courses.AnyAsync(c => c.Id == id && c.CreatedById == userId);
    }

    public Task<PaginatedList<Course>> GetAsync(SieveModel sieveModel)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedList<Course>> GetAssignedByUserId(int userId, SieveModel sieveModel)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedList<Course>> GetCreatedByUserId(int userId, SieveModel sieveModel)
    {
        throw new NotImplementedException();
    }
}