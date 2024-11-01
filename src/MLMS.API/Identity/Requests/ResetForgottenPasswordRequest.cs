namespace MLMS.API.Identity.Requests;

public class ResetForgottenPasswordRequest
{
    public string WorkId { get; set; }
    
    public string Token { get; set; }
    
    public string NewPassword { get; set; }
}