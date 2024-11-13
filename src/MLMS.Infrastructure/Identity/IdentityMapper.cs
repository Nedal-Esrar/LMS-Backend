using Microsoft.AspNetCore.Identity;
using MLMS.Domain.Entities;
using MLMS.Domain.Identity;
using MLMS.Domain.Users;
using MLMS.Infrastructure.Identity.Models;
using Riok.Mapperly.Abstractions;

namespace MLMS.Infrastructure.Identity;

[Mapper]
public static partial class IdentityMapper
{
    public static User ToDomain(this ApplicationUser userEntity)
    {
        var user = userEntity.ToDomainInternal();

        user.Role = Enum.Parse<UserRole>(userEntity.Role.Name!);

        return user;
    }
    
    [MapperIgnoreSource(nameof(userEntity.Role))]
    private static partial User ToDomainInternal(this ApplicationUser userEntity);

    [MapperIgnoreSource(nameof(user.Role))]
    public static partial ApplicationUser ToIdentityUser(this User user);
}