using ErrorOr;

namespace MLMS.Domain.Sections;

public interface ISectionRepository
{
    Task<bool> ExistsAsync(long id);
    
    Task<bool> ExistsAsync(long courseId, long id);
    
    Task<Section> CreateAsync(Section section);
    
    Task<bool> ExistsByTitleAsync(string title);
    
    Task<int> GetMaxIndexByCourseIdAsync(long courseId);
    
    Task UpdateAsync(Section section);
    
    Task<Section?> GetByIdAsync(long id);
    
    Task DeleteAsync(long id);
}