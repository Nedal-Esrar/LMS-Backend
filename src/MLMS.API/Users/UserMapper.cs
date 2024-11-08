using MLMS.API.Users.Requests;
using MLMS.Domain.Users;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.Users;

[Mapper]
public static partial class UserMapper
{
    public static partial User ToDomain(this UpdateUserRequest request);
}