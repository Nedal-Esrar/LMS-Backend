using ErrorOr;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
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

    public async Task<ErrorOr<None>> DeleteAsync(Guid id)
    {
        var fileModel = await fileRepository.GetByIdAsync(id);

        if (fileModel is null)
        {
            return FileErrors.NotFound;
        }
        
        await transactionProvider.ExecuteInTransactionAsync(async () =>
        {
            await fileRepository.DeleteAsync(id);

            await fileHandler.DeleteAsync(fileModel.Path);
        });

        return None.Value;
    }
}