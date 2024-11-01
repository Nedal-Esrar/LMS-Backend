using MLMS.Domain.Entities;

namespace MLMS.Domain.Identity;

public class UserTokens
{
    public AccessToken AccessToken { get; set; }
    
    public RefreshToken RefreshToken { get; set; }
}