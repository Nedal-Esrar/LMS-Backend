using MLMS.API.Departments;
using MLMS.API.Files;
using MLMS.API.Identity.Requests;
using MLMS.API.Identity.Responses;
using MLMS.API.Majors;
using MLMS.API.Users.Responses;
using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Models;
using MLMS.Domain.Users;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.Identity;

[Mapper]
public static partial class IdentityMapper
{
    public static partial LoginCredentials ToLoginCredentials(this LoginRequest loginRequest);

    public static partial User ToDomainUser(this RegisterRequest registerRequest);

    public static LoginResponse ToContract(this UserTokens userTokens)
    {
        return new LoginResponse
        {
            AccessToken = userTokens.AccessToken.Token,
            RefreshToken = userTokens.RefreshToken.Token
        };
    }
}