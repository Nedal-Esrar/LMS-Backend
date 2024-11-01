using ErrorOr;
using MLMS.Domain.Entities;

namespace MLMS.Domain.Departments;

public interface IDepartmentService
{
    Task<ErrorOr<Department>> CreateAsync(Department department);
    
    Task<ErrorOr<None>> DeleteAsync(int id);
    
    Task<ErrorOr<Department>> GetByIdAsync(int id);
    
    Task<ErrorOr<List<Department>>> GetAsync();
}