using ErrorOr;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Users;

namespace MLMS.Domain.Files;

public class FileService(
    IFileHandler fileHandler,
    IFileRepository fileRepository,
    IDbTransactionProvider transactionProvider) : IFileService
{
    public async Task<ErrorOr<Guid>> UploadAsync(Stream fileContentStream, File fileModel)
    {
        var filePath = await fileHandler.StoreAsync(fileContentStream, fileModel.Name);

        fileModel.Path = filePath;
        
        await fileRepository.CreateAsync(fileModel);

        return fileModel.Id;
    }
}