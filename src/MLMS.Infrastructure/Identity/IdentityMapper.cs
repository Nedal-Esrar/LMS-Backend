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
    public static partial User ToDomain(this ApplicationUser userEntity);

    public static partial ApplicationUser ToIdentityUser(this User user);
}