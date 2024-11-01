using Microsoft.AspNetCore.Identity;
using MLMS.Domain.Entities;
using MLMS.Domain.Enums;
using MLMS.Infrastructure.Identity.Models;
using Riok.Mapperly.Abstractions;

namespace MLMS.Infrastructure.Identity;

[Mapper]
public static partial class IdentityMapper
{
    public static User ToDomain(this ApplicationUser userEntity)
    {
        var user = userEntity.ToDomainInternal();
        
        user.Roles = userEntity.Roles.Select(r => Enum.Parse<UserRole>(r.Name!)).ToList();

        return user;
    }
    
    [MapperIgnoreSource(nameof(ApplicationUser.Roles))]
    private static partial User ToDomainInternal(this ApplicationUser userEntity);

    [MapperIgnoreSource(nameof(User.Roles))]
    public static partial ApplicationUser ToIdentityUser(this User user);
}