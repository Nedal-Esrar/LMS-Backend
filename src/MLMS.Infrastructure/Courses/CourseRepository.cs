using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Courses;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Users;
using MLMS.Infrastructure.Common;
using Sieve.Models;
using Sieve.Services;

namespace MLMS.Infrastructure.Courses;

public class CourseRepository(
    LmsDbContext context,
    IUserContext userContext,
    ISieveProcessor sieveProcessor) : ICourseRepository
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

    public async Task<Course?> GetDetailedByIdAsync(long id)
    {
        var userId = userContext.Id!.Value;
        
        var query = context.Courses.Where(c => c.Id == id)
            .Include(x => x.UsersCourses.Where(uc => uc.UserId == userId))
            .Include(x => x.Assignments)
                .ThenInclude(x => x.Major)
            .Include(x => x.Sections)
                .ThenInclude(x => x.SectionParts)
                    .ThenInclude(x => x.Exam)
                        .ThenInclude(x => x.Questions)
                            .ThenInclude(x => x.Choices)
            .Include(x => x.Sections)
                .ThenInclude(x => x.SectionParts)
                    .ThenInclude(x => x.Exam)
                        .ThenInclude(x => x.Questions)
                            .ThenInclude(x => x.Image)
            .Include(x => x.Sections)
                .ThenInclude(x => x.SectionParts)
                    .ThenInclude(x => x.File)
            .Include(x => x.Sections)
                .ThenInclude(x => x.SectionParts)
                    .ThenInclude(x => x.UserExamStates.Where(ue => ue.UserId == userId))
            .Include(x => x.Sections)
                .ThenInclude(x => x.SectionParts)
                    .ThenInclude(x => x.UserSectionPartStatuses.Where(usp => usp.UserId == userId));
        
        var courseWithCreatedByUserQuery = from c in query
            join u in context.Users on c.CreatedById equals u.Id
            select new Course
            {
                Id = c.Id,
                Name = c.Name,
                ExpectedTimeToFinishHours = c.ExpectedTimeToFinishHours,
                ExpirationMonths = c.ExpirationMonths,
                CreatedAtUtc = c.CreatedAtUtc,
                UpdatedAtUtc = c.UpdatedAtUtc,
                CreatedById = c.CreatedById,
                CreatedBy = new User
                {
                    Id = u.Id, 
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName
                },
                Sections = c.Sections,
                UsersCourses = c.UsersCourses,
                Assignments = c.Assignments
            };
        
        return await courseWithCreatedByUserQuery.AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByIdAsync(long id)
    {
        return await context.Courses.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> IsCreatedByUserAsync(long id, int userId)
    {
        return await context.Courses.AnyAsync(c => c.Id == id && c.CreatedById == userId);
    }

    public async Task<PaginatedList<Course>> GetAsync(SieveModel sieveModel)
    {
        var query = from c in context.Courses
            join u in context.Users on c.CreatedById equals u.Id
            select new Course
            {
                Id = c.Id,
                Name = c.Name,
                ExpectedTimeToFinishHours = c.ExpectedTimeToFinishHours,
                ExpirationMonths = c.ExpirationMonths,
                CreatedAtUtc = c.CreatedAtUtc,
                UpdatedAtUtc = c.UpdatedAtUtc,
                CreatedById = c.CreatedById,
                CreatedBy = new User
                {
                    Id = u.Id, 
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName
                }
            };

        var totalItems = await sieveProcessor.Apply(sieveModel, query, applyPagination: false).CountAsync();

        query = sieveProcessor.Apply(sieveModel, query);

        var result = await query.AsNoTracking()
            .ToListAsync();

        return new PaginatedList<Course>
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

    public async Task<PaginatedList<Course>> GetAssignedByUserId(int userId, SieveModel sieveModel)
    {
        var query = from c in context.Courses
                .Include(c => c.UsersCourses.Where(uc => uc.UserId == userId))
                .Include(c => c.Sections)
                    .ThenInclude(s => s.SectionParts)
                        .ThenInclude(sp => sp.UserSectionPartStatuses.Where(usp => usp.UserId == userId))
            join uc in context.UserCourses on c.Id equals uc.CourseId
            where uc.UserId == userId
            select c;

        var totalItems = await sieveProcessor.Apply(sieveModel, query, applyPagination: false).CountAsync();

        query = sieveProcessor.Apply(sieveModel, query);

        var result = await query.AsNoTracking()
            .ToListAsync();

        return new PaginatedList<Course>
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

    public async Task<PaginatedList<Course>> GetCreatedByUserId(int userId, SieveModel sieveModel)
    {
        var query = from c in context.Courses
            join u in context.Users on c.CreatedById equals u.Id
            where c.CreatedById == userId
            select new Course
            {
                Id = c.Id,
                Name = c.Name,
                ExpectedTimeToFinishHours = c.ExpectedTimeToFinishHours,
                ExpirationMonths = c.ExpirationMonths,
                CreatedAtUtc = c.CreatedAtUtc,
                UpdatedAtUtc = c.UpdatedAtUtc,
                CreatedById = c.CreatedById,
                CreatedBy = new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName
                }
            };

        var totalItems = await sieveProcessor.Apply(sieveModel, query, applyPagination: false).CountAsync();

        query = sieveProcessor.Apply(sieveModel, query);

        var result = await query.AsNoTracking()
            .ToListAsync();

        return new PaginatedList<Course>
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

    public async Task<Course?> GetByIdAsync(long id)
    {
        return await context.Courses.FindAsync(id);
    }

    public async Task UpdateAsync(Course course)
    {
        context.Courses.Update(course);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var course = await GetByIdAsync(id);

        if (course is null)
        {
            return;
        }

        context.Courses.Remove(course);

        await context.SaveChangesAsync();
    }
}