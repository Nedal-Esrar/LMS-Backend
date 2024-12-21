using MLMS.Domain.UserSectionParts;

namespace MLMS.Domain.SectionParts;

public interface ISectionPartRepository
{
    Task<SectionPart> CreateAsync(SectionPart sectionPart);
    
    Task<int> GetMaxIndexBySectionIdAsync(long sectionId);
    
    Task<bool> ExistsAsync(long sectionId, long id);
    
    Task DeleteAsync(long id);
    
    Task<SectionPart?> GetByIdAsync(long id);
    
    Task UpdateAsync(SectionPart sectionPart);
    
    Task ToggleUserDoneStatusAsync(int userId, long id);
    
    Task<List<UserExamStateForSectionPart>> GetExamStatusesByCourseAndUserAsync(long id, int userId);
}