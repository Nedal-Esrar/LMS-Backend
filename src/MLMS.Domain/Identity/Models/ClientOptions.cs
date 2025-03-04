namespace MLMS.Domain.Identity.Models;

public class ClientOptions
{
    public string BaseUrl { get; set; } = string.Empty;

    public string LoginRoute { get; set; } = string.Empty;

    public string ResetPasswordRoute { get; set; } = string.Empty;
}