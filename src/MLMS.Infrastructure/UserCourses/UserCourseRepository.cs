using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Users;
using MLMS.Domain.UsersCourses;
using MLMS.Infrastructure.Common;
using Sieve.Models;
using Sieve.Services;

namespace MLMS.Infrastructure.UserCourses;

public class UserCourseRepository(
    LmsDbContext context,
    ISieveProcessor sieveProcessor,
    IUserContext userContext) : IUserCourseRepository
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

    public async Task<List<UserCourse>> GetStatesForFinishedCoursesWithExpirationAsync()
    {
        var query = from uc in context.UserCourses
                    join c in context.Courses on uc.CourseId equals c.Id
                    join u in context.Users on uc.UserId equals u.Id
                    where uc.Status == UserCourseStatus.Finished && c.ExpirationMonths != null
                    select new UserCourse
                    {
                        UserId = uc.UserId,
                        CourseId = uc.CourseId,
                        Course = uc.Course,
                        Status = uc.Status,
                        FinishedAtUtc = uc.FinishedAtUtc,
                        User = new User { Id = u.Id, Email = u.Email }
                    };

        return await query.AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(List<UserCourse> userCourses)
    {
        context.UserCourses.UpdateRange(userCourses);

        await context.SaveChangesAsync();
    }

    public async Task<PaginatedList<UserCourse>> GetByCourseIdAsync(long courseId, bool getOnlyCreatedByCurrentUser, SieveModel sieveModel)
    {
        var baseQuery = from uc in context.UserCourses
            join u in context.Users on uc.UserId equals u.Id
            where uc.CourseId == courseId
            select new { uc, u };

        if (getOnlyCreatedByCurrentUser)
        {
            baseQuery = baseQuery.Where(x => x.uc.Course.CreatedById == userContext.Id!.Value);
        }

        var query = from q in baseQuery
            select new UserCourse
            {
                UserId = q.uc.UserId,
                CourseId = q.uc.CourseId,
                StartedAtUtc = q.uc.StartedAtUtc,
                FinishedAtUtc = q.uc.FinishedAtUtc,
                Status = q.uc.Status,
                User = new User
                {
                    Id = q.u.Id,
                    FirstName = q.u.FirstName,
                    LastName = q.u.LastName,
                    MiddleName = q.u.MiddleName
                }
            };

        var totalItems = await sieveProcessor.Apply(sieveModel, query, applyPagination: false).CountAsync();

        query = sieveProcessor.Apply(sieveModel, query);

        var result = await query.AsNoTracking()
            .ToListAsync();

        return new PaginatedList<UserCourse>
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

    public async Task DeleteAsync(List<UserCourse> userCourses)
    {
        context.UserCourses.RemoveRange(userCourses);
        
        await context.SaveChangesAsync();
    }
}