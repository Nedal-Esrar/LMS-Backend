using ErrorOr;
using MLMS.Domain.Entities;
using MLMS.Domain.Identity;

namespace MLMS.Domain.Users;

public interface IUserService
{
    Task<ErrorOr<None>> UpdateAsync(int id, User user);

    Task<ErrorOr<User>> GetByIdAsync(int id);

    Task<ErrorOr<None>> DeleteAsync(int id);
}