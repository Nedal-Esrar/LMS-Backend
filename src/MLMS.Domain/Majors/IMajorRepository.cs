using ErrorOr;
using MLMS.Domain.Common.Models;
using Sieve.Models;

namespace MLMS.Domain.Majors;

public interface IMajorRepository
{
    Task<Major> CreateAsync(Major major);
    
    Task<bool> ExistsAsync(int departmentId, int id);
    
    Task DeleteAsync(int departmentId, int id);
    
    Task<Major?> GetByIdAsync(int departmentId, int id);
    
    Task<PaginatedList<Major>> GetByDepartmentAsync(int departmentId, SieveModel sieveModel);
    
    Task<bool> ExistsByNameAsync(int majorDepartmentId, string majorName);
    
    Task UpdateAsync(Major major);
}