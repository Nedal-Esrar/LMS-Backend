using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Identity.Models;

public class UserTokens
{
    public AccessToken AccessToken { get; set; }
    
    public RefreshToken RefreshToken { get; set; }
}