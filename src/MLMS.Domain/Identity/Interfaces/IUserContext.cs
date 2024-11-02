using MLMS.Domain.Enums;

namespace MLMS.Domain.Identity.Interfaces;

public interface IUserContext
{
    public int? Id { get; }
    
    public UserRole? Role { get; }
    
    public string Name { get; }
    
    public string Email { get; }
}