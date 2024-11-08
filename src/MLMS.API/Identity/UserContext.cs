using System.Security.Claims;
using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Interfaces;

namespace MLMS.API.Identity;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public int? Id { get; } =
        int.TryParse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!, out var id) ? id : null;

    public UserRole? Role { get; } =
        Enum.TryParse<UserRole>(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role)!, out var role) ? role : null;

    public string Name { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)!;

    public string Email { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)!;
}