using ErrorOr;
using MLMS.Domain.Entities;

namespace MLMS.Domain.Majors;

public interface IMajorService
{
    Task<ErrorOr<Major>> CreateAsync(Major major);
    
    Task<ErrorOr<None>> DeleteAsync(int departmentId, int id);
    
    Task<ErrorOr<Major>> GetByIdAsync(int departmentId, int id);
    
    Task<ErrorOr<List<Major>>> GetByDepartmentAsync(int departmentId);
}