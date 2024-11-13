using MLMS.API.Files.Responses;
using Riok.Mapperly.Abstractions;
using File = MLMS.Domain.Files.File;

namespace MLMS.API.Files;

[Mapper]
public static partial class FileMapper
{
    public static File ToDomain(this IFormFile file)
    {
        return new File
        {
            Id = Guid.NewGuid(),
            Name = file.FileName,
            Extension = Path.GetExtension(file.FileName),
            ContentType = file.ContentType
        };
    }

    public static partial FileResponse? ToContract(this File file);
}