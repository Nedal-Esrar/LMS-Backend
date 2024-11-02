using ErrorOr;

namespace MLMS.Domain.Majors;

public interface IMajorRepository
{
    Task<Major> CreateAsync(Major major);
    
    Task<bool> ExistsAsync(int departmentId, int id);
    
    Task DeleteAsync(int departmentId, int id);
    
    Task<Major?> GetByIdAsync(int departmentId, int id);
    
    Task<List<Major>> GetByDepartmentAsync(int departmentId);
    
    Task<bool> ExistsByNameAsync(int majorDepartmentId, string majorName);
}