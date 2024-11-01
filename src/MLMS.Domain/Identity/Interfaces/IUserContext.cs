namespace MLMS.Domain.Identity.Interfaces;

public interface IUserContext
{
    public int Id { get; }
    
    public string Name { get; }
    
    public string Email { get; }
}