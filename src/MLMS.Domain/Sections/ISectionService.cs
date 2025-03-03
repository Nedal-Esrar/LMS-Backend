using ErrorOr;
using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Sections;

public interface ISectionService
{
    Task<ErrorOr<Section>> CreateAsync(Section section);
    
    Task<ErrorOr<None>> UpdateAsync(long id, Section section);
    
    Task<ErrorOr<Section>> GetByIdAsync(long courseId, long id);
    
    Task<ErrorOr<None>> DeleteAsync(long courseId, long id);
    
    Task<ErrorOr<None>> CheckExistenceAsync(long id);
}