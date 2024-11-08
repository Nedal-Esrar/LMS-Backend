using MLMS.Domain.Entities;
using MLMS.Domain.Users;

namespace MLMS.Domain.Identity.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    
    Task CreateAsync(User user, string password);
    
    Task<bool> ExistsByWorkIdAsync(string workId);
    
    Task<User?> GetByWorkIdAsync(string workId);
    
    Task UpdateAsync(User userToUpdate);
    
    Task<bool> ExistsAsync(int id);
    
    Task DeleteAsync(int id);
}