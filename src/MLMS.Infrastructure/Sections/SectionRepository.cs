using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Sections;
using MLMS.Domain.Users;
using MLMS.Infrastructure.Common;
using MLMS.Infrastructure.Identity;

namespace MLMS.Infrastructure.Sections;

public class SectionRepository(LmsDbContext context) : ISectionRepository
{
    public async Task<bool> ExistsAsync(long id)
    {
        return await context.Sections.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsAsync(long courseId, long id)
    {
        return await context.Sections.AnyAsync(x => x.CourseId == courseId && x.Id == id);
    }

    public async Task<Section> CreateAsync(Section section)
    {
        await context.Sections.AddAsync(section);
        
        await context.SaveChangesAsync();
        
        return section;
    }

    public async Task<bool> ExistsByTitleAsync(string title)
    {
        return await context.Sections.AnyAsync(x => x.Title == title);
    }

    public async Task<int> GetMaxIndexByCourseIdAsync(long courseId)
    {
        return await context.Sections
            .Where(x => x.CourseId == courseId)
            .Select(x => x.Index)
            .DefaultIfEmpty()
            .MaxAsync();
    }

    public async Task UpdateAsync(Section section)
    {
        context.Sections.Update(section);
        
        await context.SaveChangesAsync();
    }

    public async Task<Section?> GetByIdAsync(long id)
    {
        return await context.Sections.FindAsync(id);
    }

    public async Task DeleteAsync(long id)
    {
        var section = await context.Sections.FindAsync(id);

        if (section is null)
        {
            return;
        }
        
        context.Sections.Remove(section);
        
        await context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsersBySectionIdAsync(long sectionId)
    {
        var query = from u in context.Users
            join uc in context.UserCourses on u.Id equals uc.UserId
            join c in context.Courses on uc.CourseId equals c.Id
            join s in context.Sections on c.Id equals s.CourseId
            where s.Id == sectionId
            select u;

        return (await query.AsNoTracking()
            .ToListAsync())
            .Select(u => u.ToDomain())
            .ToList();
    }
}