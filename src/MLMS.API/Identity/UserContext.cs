using System.Security.Claims;
using MLMS.Domain.Enums;
using MLMS.Domain.Identity.Interfaces;

namespace MLMS.API.Identity;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public int Id { get; } =
        int.Parse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public UserRole Role { get; } =
        Enum.Parse<UserRole>(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role)!);

    public string Name { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)!;

    public string Email { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)!;
}