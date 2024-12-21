using MLMS.Domain.SectionParts;
using MLMS.Domain.UserSectionParts;

namespace MLMS.Infrastructure.SectionParts;

public class SectionPartRepository : ISectionPartRepository
{
    public Task<SectionPart> CreateAsync(SectionPart sectionPart)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetMaxIndexBySectionIdAsync(long sectionId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(long sectionId, long id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<SectionPart?> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(SectionPart sectionPart)
    {
        throw new NotImplementedException();
    }

    public Task ToggleUserDoneStatusAsync(int userId, long id)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserExamStateForSectionPart>> GetExamStatusesByCourseAndUserAsync(long id, int userId)
    {
        throw new NotImplementedException();
    }
}