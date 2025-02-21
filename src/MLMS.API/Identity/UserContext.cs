using System.Security.Claims;
using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Enums;
using MLMS.Domain.Identity.Interfaces;

namespace MLMS.API.Identity;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public int Id => int.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));

    public UserRole Role => Enum.Parse<UserRole>(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role)!);

    public string Name => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)!;

    public string Email => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)!;
}