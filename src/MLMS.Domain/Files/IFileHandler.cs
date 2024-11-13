namespace MLMS.Domain.Files;

public interface IFileHandler
{
    Task<string> StoreAsync(Stream fileContentStream, string fileName);
}