using MLMS.API.Identity.Requests;
using MLMS.API.Identity.Responses;
using MLMS.Domain.Entities;
using MLMS.Domain.Identity;
using MLMS.Domain.Models;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.Identity;

[Mapper]
public static partial class IdentityMapper
{
    public static partial LoginCredentials ToLoginCredentials(this LoginRequest loginRequest);

    public static User ToDomainUser(this RegisterRequest registerRequest)
    {
        var user = registerRequest.ToDomainUserInternal();
        
        user.Roles.Add(registerRequest.Role);

        return user;
    }

    private static partial User ToDomainUserInternal(this RegisterRequest registerRequest);

    public static LoginResponse ToContract(this UserTokens userTokens)
    {
        return new LoginResponse
        {
            AccessToken = userTokens.AccessToken.Token,
            RefreshToken = userTokens.RefreshToken.Token
        };
    }
}