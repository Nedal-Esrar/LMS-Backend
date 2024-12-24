using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.SectionParts;
using MLMS.Domain.UserSectionParts;
using MLMS.Infrastructure.Common;

namespace MLMS.Infrastructure.SectionParts;

public class SectionPartRepository(
    LmsDbContext context,
    IUserContext userContext) : ISectionPartRepository
{
    public async Task<SectionPart> CreateAsync(SectionPart sectionPart)
    {
        await context.SectionParts.AddAsync(sectionPart);
        
        await context.SaveChangesAsync();
        
        return sectionPart;
    }

    public async Task<int> GetMaxIndexBySectionIdAsync(long sectionId)
    {
        return await context.SectionParts
            .Where(x => x.SectionId == sectionId)
            .Select(x => x.Index)
            .DefaultIfEmpty()
            .MaxAsync();
    }

    public async Task<bool> ExistsAsync(long sectionId, long id)
    {
        return await context.SectionParts.AnyAsync(x => x.SectionId == sectionId && x.Id == id);
    }

    public async Task DeleteAsync(long id)
    {
        var sectionPart = await context.SectionParts.FindAsync(id);

        if (sectionPart is null)
        {
            return;
        }
        
        context.SectionParts.Remove(sectionPart);
        
        await context.SaveChangesAsync();
    }

    public async Task<SectionPart?> GetByIdAsync(long id)
    {
        var userId = userContext.Id!.Value;

        var query = context.SectionParts.Where(sp => sp.Id == id)
            .Include(x => x.File)
            .Include(x => x.Questions)
                .ThenInclude(x => x.Choices)
            .Include(x => x.UserExamStates.Where(ue => ue.UserId == id))
            .Include(x => x.UserSectionPartStatuses.Where(usp => usp.UserId == userId))
            .AsSplitQuery()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(SectionPart sectionPart)
    {
        context.SectionParts.Update(sectionPart);
        
        await context.SaveChangesAsync();
    }

    public async Task ToggleUserDoneStatusAsync(int userId, long id)
    {
        await context.UserSectionPartDoneRelations
            .Where(x => x.UserId == userId && x.SectionPartId == id)
            .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsDone, x => !x.IsDone));
    }

    public async Task<List<UserSectionPartExamState>> GetExamStatusesByCourseAndUserAsync(long id, int userId)
    {
        var query = from c in context.Courses
            join s in context.Sections on c.Id equals s.CourseId
            join sp in context.SectionParts on s.Id equals sp.SectionId
            join usp in context.UserSectionPartExamStateRelations on sp.Id equals usp.SectionPartId
            where c.Id == id && usp.UserId == userId
            select usp;
        
        return await query.AsNoTracking()
            .ToListAsync();
    }

    public async Task CreateDoneStatesAsync(List<UserSectionPartDone> doneStates)
    {
        context.UserSectionPartDoneRelations.AddRange(doneStates);
        
        await context.SaveChangesAsync();
    }

    public async Task CreateExamStatesAsync(List<UserSectionPartExamState> examStates)
    {
        context.UserSectionPartExamStateRelations.AddRange(examStates);
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteExamStatesByIdAsync(long id)
    {
        await context.UserSectionPartExamStateRelations
            .Where(use => use.SectionPartId == id)
            .ExecuteDeleteAsync();
    }
}