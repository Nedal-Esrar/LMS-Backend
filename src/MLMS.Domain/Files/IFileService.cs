using ErrorOr;

namespace MLMS.Domain.Files;

public interface IFileService
{
    Task<ErrorOr<Guid>> UploadAsync(Stream fileContentStream, File fileModel);
}