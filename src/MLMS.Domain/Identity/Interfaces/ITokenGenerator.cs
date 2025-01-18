using MLMS.Domain.Common.Models;
using MLMS.Domain.Identity.Models;
using MLMS.Domain.Users;

namespace MLMS.Domain.Identity.Interfaces;

public interface ITokenGenerator
{
    AccessToken GenerateAccessToken(User user);

    RefreshToken GenerateRefreshToken(int userId);
}