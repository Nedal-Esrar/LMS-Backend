using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.UsersCourses;

namespace MLMS.Domain.SectionParts;

public interface ISectionPartService
{
    Task<ErrorOr<SectionPart>> CreateAsync(SectionPart sectionPart);
    
    Task<ErrorOr<None>> DeleteAsync(long sectionId, long id);
    
    Task<ErrorOr<None>> UpdateAsync(long id, SectionPart updatedSectionPart);
    
    Task<ErrorOr<SectionPart>> GetByIdAsync(long sectionId, long id);
    
    Task<ErrorOr<None>> ChangeUserSectionPartStatusAsync(long sectionId, long id, SectionPartStatus status);
    
    Task<ErrorOr<None>> ResetDoneStatesByUserCoursesAsync(List<UserCourse> userCourses);
}