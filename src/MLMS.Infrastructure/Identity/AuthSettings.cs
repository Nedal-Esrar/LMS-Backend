namespace MLMS.Infrastructure.Identity;

public class AuthSettings
{
    public string Secret { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;
    
    public double AccessTokenLifeTimeMinutes { get; set; }
    
    public double RefreshTokenLifeTimeDays { get; set; }
    
    public double ResetPasswordTokenLifeTimeMinutes { get; set; }
}