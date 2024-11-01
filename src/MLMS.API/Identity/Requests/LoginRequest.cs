namespace MLMS.API.Identity.Requests;

public class LoginRequest
{
    public string WorkId { get; set; }
    
    public string Password { get; set; }
}