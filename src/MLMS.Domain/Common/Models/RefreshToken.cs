namespace MLMS.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    
    public string Token { get; set; }
    
    public DateTime Expiration { get; set; }
    
    public int UserId { get; set; }
}