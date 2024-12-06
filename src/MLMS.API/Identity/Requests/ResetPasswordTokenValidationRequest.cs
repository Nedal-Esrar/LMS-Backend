namespace MLMS.API.Identity.Requests;

public class ResetPasswordTokenValidationRequest
{
    public string WorkId { get; set; }
    
    public string Token { get; set; }
}