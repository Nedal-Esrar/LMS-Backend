using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Identity;
using Sieve.Models;

namespace MLMS.Domain.Users;

public interface IUserService
{
    Task<ErrorOr<None>> UpdateAsync(int id, User user);

    Task<ErrorOr<User>> GetByIdAsync(int id);

    Task<ErrorOr<None>> DeleteAsync(int id);
    
    Task<ErrorOr<PaginatedList<User>>> GetAsync(SieveModel sieveModel);
    
    Task<ErrorOr<None>> SetProfilePictureAsync(Guid imageId);
}