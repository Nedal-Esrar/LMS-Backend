using MLMS.Domain.Entities;

namespace MLMS.Domain.Identity.Interfaces;

public interface ITokenGenerator
{
    AccessToken GenerateAccessToken(User user);

    RefreshToken GenerateRefreshToken(int userId);
}