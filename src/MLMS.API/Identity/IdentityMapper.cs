using MLMS.API.Departments;
using MLMS.API.Identity.Requests;
using MLMS.API.Identity.Responses;
using MLMS.API.Majors;
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

    public static UserResponse ToContract(this User user)
    {
        var userContract = ToContractInternal(user);
        
        userContract.Role = user.Roles.First();
        userContract.Major = user.Major.ToContract();
        userContract.Department = user.Department.ToContract();

        return userContract;
    }
    
    [MapperIgnoreSource(nameof(User.Roles))]
    [MapperIgnoreSource(nameof(User.Major))]
    [MapperIgnoreSource(nameof(User.Department))]
    private static partial UserResponse ToContractInternal(this User user);
}