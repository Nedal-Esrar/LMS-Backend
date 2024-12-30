using ErrorOr;
using MLMS.Domain.Common.Models;

namespace MLMS.Domain.SectionParts;

public interface ISectionPartService
{
    Task<ErrorOr<SectionPart>> CreateAsync(SectionPart sectionPart);
    
    Task<ErrorOr<None>> DeleteAsync(long sectionId, long id);
    
    Task<ErrorOr<None>> UpdateAsync(long id, SectionPart updatedSectionPart);
    
    Task<ErrorOr<SectionPart>> GetByIdAsync(long sectionId, long id);
    
    Task<ErrorOr<None>> ToggleUserDoneStatusAsync(long sectionId, long id);
}