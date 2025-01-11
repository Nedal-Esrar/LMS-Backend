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
                .ThenInclude(x => x.Exam)
                .ThenInclude(x => x.UserExamStates.Where(ue => ue.UserId == userId))
            .Include(x => x.Sections)
                .ThenInclude(x => x.SectionParts)
                .ThenInclude(x => x.UserSectionPartStatuses.Where(usp => usp.UserId == userId))
            .Include(x => x.Sections)
                .ThenInclude(x => x.SectionParts)
                .ThenInclude(x => x.Exam)
                .ThenInclude(x => x.ExamSessions.Where(ue => ue.UserId == userId && ue.IsDone)
                    .OrderByDescending(es => es.StartDateUtc)
                    .Take(1));

        var course = await query.AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (course is null)
        {
            return null;
        }

        course.CreatedBy = await context.Users
            .Where(u => u.Id == course.CreatedById)
            .Select(u => new User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName
            })
            .FirstOrDefaultAsync();

        return course;
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
            join u in context.Users on c.CreatedById equals u.Id into userGroup
            from u in userGroup.DefaultIfEmpty()
            select new Course
            {
                Id = c.Id,
                Name = c.Name,
                ExpectedTimeToFinishHours = c.ExpectedTimeToFinishHours,
                ExpirationMonths = c.ExpirationMonths,
                CreatedAtUtc = c.CreatedAtUtc,
                UpdatedAtUtc = c.UpdatedAtUtc,
                CreatedById = c.CreatedById,
                CreatedBy = u == null ? null : new User
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

    public async Task<List<long>> GetExamIdsByIdAsync(long id)
    {
        var query = from c in context.Courses
            join s in context.Sections on c.Id equals s.CourseId
            join sp in context.SectionParts on s.Id equals sp.SectionId
            join e in context.Exams on sp.Id equals e.SectionPartId
            where c.Id == id
            select e.Id;

        return await query.ToListAsync();
    }
}