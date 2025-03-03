using ErrorOr;
using MLMS.Domain.Common.Models;
using Sieve.Models;

namespace MLMS.Domain.Users;

public interface IUserService
{
    Task<ErrorOr<None>> UpdateAsync(int id, User user);

    Task<ErrorOr<User>> GetByIdAsync(int id);
    
    Task<ErrorOr<PaginatedList<User>>> GetAsync(SieveModel sieveModel);
    
    Task<ErrorOr<None>> SetProfilePictureAsync(Guid imageId);
    
    Task<ErrorOr<None>> ChangeUserStatusAsync(int id, bool requestIsActive);
    
    Task<ErrorOr<List<User>>> GetBySectionIdAsync(long sectionId);
    
    Task<ErrorOr<bool>> CheckExistenceByWorkIdAsync(string workId);
    
    Task<ErrorOr<int>> CreateAsync(User user, string password);
    
    Task<ErrorOr<User>> GetByWorkIdAsync(string workId);
    
    Task<ErrorOr<None>> CheckExistenceByIdAsync(int id);
    
    Task<ErrorOr<None>> CheckIfSubAdminAsync(int subAdminId);
    
    Task<ErrorOr<List<User>>> GetByMajorsAsync(List<int> majorIds);
}