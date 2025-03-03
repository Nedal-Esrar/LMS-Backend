using ErrorOr;
using MLMS.Domain.Common.Models;
using Sieve.Models;

namespace MLMS.Domain.Majors;

public interface IMajorService
{
    Task<ErrorOr<Major>> CreateAsync(Major major);
    
    Task<ErrorOr<None>> DeleteAsync(int departmentId, int id);
    
    Task<ErrorOr<Major>> GetByIdAsync(int departmentId, int id);
    
    Task<ErrorOr<PaginatedList<Major>>> GetByDepartmentAsync(int departmentId, SieveModel sieveModel);
    
    Task<ErrorOr<None>> UpdateAsync(int departmentId, int id, Major major);
    
    Task<ErrorOr<None>> CheckExistenceAsync(int departmentId, int id);
}