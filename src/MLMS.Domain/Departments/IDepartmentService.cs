using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Entities;
using Sieve.Models;

namespace MLMS.Domain.Departments;

public interface IDepartmentService
{
    Task<ErrorOr<Department>> CreateAsync(Department department);
    
    Task<ErrorOr<None>> DeleteAsync(int id);
    
    Task<ErrorOr<Department>> GetByIdAsync(int id);
    
    Task<ErrorOr<PaginatedList<Department>>> GetAsync(SieveModel sieveModel);
    
    Task<ErrorOr<None>> UpdateAsync(int id, Department toDomain);
}