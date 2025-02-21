using MLMS.Domain.Identity.Enums;

namespace MLMS.API.Common;

public static class AuthorizationPolicies
{
    public const string SuperAdmin = nameof(UserRole.Admin);
    public const string Admin = $"{nameof(UserRole.SubAdmin)},{nameof(UserRole.Admin)}";
    public const string Staff = nameof(UserRole.Staff);
}