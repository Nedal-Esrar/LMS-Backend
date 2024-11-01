using MLMS.Domain.Entities;
using MLMS.Domain.Models;

namespace MLMS.Domain.Identity.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    
    Task CreateAsync(User user, string password);
    
    Task<bool> ExistsByWorkIdAsync(string workId);
    
    Task<User?> GetByWorkIdAsync(string workId);
}