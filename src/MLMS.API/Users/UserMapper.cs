using MLMS.API.Departments;
using MLMS.API.Files;
using MLMS.API.Majors;
using MLMS.API.Users.Requests;
using MLMS.API.Users.Responses;
using MLMS.Domain.Users;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.Users;

[Mapper]
public static partial class UserMapper
{
    public static partial User ToDomain(this UpdateUserRequest request);
    
    public static UserResponse ToContract(this User user)
    {
        var userContract = ToContractInternal(user);
        
        userContract.Major = user.Major?.ToContract();
        userContract.Department = user.Department?.ToContract();
        userContract.ProfilePicture = user.ProfilePicture?.ToContract();

        return userContract;
    }
    
    [MapperIgnoreSource(nameof(User.Major))]
    [MapperIgnoreSource(nameof(User.Department))]
    [MapperIgnoreSource(nameof(User.ProfilePicture))]
    private static partial UserResponse ToContractInternal(this User user);
}