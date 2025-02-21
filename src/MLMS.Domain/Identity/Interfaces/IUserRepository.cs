using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Users;
using Sieve.Models;

namespace MLMS.Domain.Identity.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    
    Task<int> CreateAsync(User user, string password);
    
    Task<bool> ExistsByWorkIdAsync(string workId);
    
    Task<User?> GetByWorkIdAsync(string workId);
    
    Task UpdateAsync(User userToUpdate);
    
    Task<bool> ExistsAsync(int id);
    
    Task<PaginatedList<User>> GetAsync(SieveModel sieveModel);
    
    Task<List<User>> GetByMajorsAsync(List<int> majors);
    
    Task<bool> ExistsByIdAsync(int userId);
    
    Task<bool> IsSubAdminAsync(int userId);
    
    Task ChangeUserStatusAsync(int id, bool isActive);
}