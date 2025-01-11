using MLMS.Domain.UsersCourses;
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
    
    Task SetUserDoneStatusAsync(int userId, long id, bool isDone);
    
    Task<List<UserExamState>> GetExamStatusesByCourseAndUserAsync(long id, int userId);
    
    Task CreateDoneStatesAsync(List<UserSectionPartDone> doneStates);
    
    Task CreateExamStatesAsync(List<UserExamState> examStates);
    
    Task DeleteExamStatesByIdAsync(long id);
    
    Task ResetDoneStatesByUserCoursesAsync(List<UserCourse> expiredUserCourses);
}