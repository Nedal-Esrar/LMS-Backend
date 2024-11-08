using MLMS.Domain.Entities;
using MLMS.Domain.Users;

namespace MLMS.Domain.Identity.Interfaces;

public interface ITokenGenerator
{
    AccessToken GenerateAccessToken(User user);

    RefreshToken GenerateRefreshToken(int userId);
}