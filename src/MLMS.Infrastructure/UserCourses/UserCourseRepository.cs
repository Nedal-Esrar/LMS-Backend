using Microsoft.EntityFrameworkCore;
using MLMS.Domain.UsersCourses;
using MLMS.Infrastructure.Common;

namespace MLMS.Infrastructure.UserCourses;

public class UserCourseRepository(LmsDbContext context) : IUserCourseRepository
{
    public async Task CreateAsync(List<UserCourse> userCourseEntities)
    {
        context.UserCourses.AddRange(userCourseEntities);
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteByCourseAndUserIdsAsync(long id, List<int> userIds)
    {
        await context.UserCourses
            .Where(uc => uc.CourseId == id && userIds.Contains(uc.UserId))
            .ExecuteDeleteAsync();
    }

    public async Task<bool> ExistsByCourseAndUserIdsAsync(long id, int userId)
    {
        return await context.UserCourses.AnyAsync(uc => uc.CourseId == id && uc.UserId == userId);
    }

    public async Task<UserCourse> GetByUserAndCourseAsync(long id, int userId)
    {
        return await context.UserCourses
            .Where(uc => uc.CourseId == id && uc.UserId == userId)
            .FirstOrDefaultAsync()!;
    }

    public async Task UpdateAsync(UserCourse userCourse)
    {
        context.UserCourses.Update(userCourse);
        
        await context.SaveChangesAsync();
    }

    public async Task<List<UserCourse>> GetByUserIdAsync(int userId)
    {
        return await context.UserCourses
            .Where(uc => uc.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }
}