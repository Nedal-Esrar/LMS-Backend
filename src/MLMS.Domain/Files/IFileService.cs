using ErrorOr;
using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Files;

public interface IFileService
{
    Task<ErrorOr<Guid>> UploadAsync(Stream fileContentStream, File fileModel);
    
    Task<ErrorOr<None>> DeleteAsync(Guid id);
    
    Task<ErrorOr<File>> GetByIdAsync(Guid id);
    
    Task<ErrorOr<None>> CheckExistenceAsync(Guid id);
}