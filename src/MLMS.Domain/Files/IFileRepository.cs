namespace MLMS.Domain.Files;

public interface IFileRepository
{
    Task CreateAsync(File file);
    
    Task<File?> GetByIdAsync(Guid id);
    
    Task DeleteAsync(Guid id);
    
    Task<bool> ExistsAsync(Guid id);
}