using MLMS.Domain.Common.Models;
using Sieve.Models;

namespace MLMS.Domain.Departments;

public interface IDepartmentRepository
{
    Task<Department> CreateAsync(Department department);
    
    Task<bool> ExistsAsync(int id);
    
    Task DeleteAsync(int id);
    
    Task<Department?> GetByIdAsync(int id);
    
    Task<PaginatedList<Department>> GetAsync(SieveModel sieveModel);
    
    Task<bool> ExistsByNameAsync(string departmentName);
    
    Task UpdateAsync(Department department);
}