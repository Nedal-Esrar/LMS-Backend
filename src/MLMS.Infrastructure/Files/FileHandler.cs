using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MLMS.Domain.Files;
using File = System.IO.File;

namespace MLMS.Infrastructure.Files;

public class FileHandler(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : IFileHandler
{
    public async Task<string> StoreAsync(Stream fileContentStream, string fileName)
    {
        // guarantee uniqueness.
        fileName = Guid.NewGuid() + fileName;

        var uploadedFilesPath = Path.Combine(webHostEnvironment.WebRootPath, "uploaded-files");

        if (!Directory.Exists(uploadedFilesPath))
        {
            Directory.CreateDirectory(uploadedFilesPath);
        }

        var filePath = Path.Combine(webHostEnvironment.WebRootPath, $"uploaded-files/{fileName}");

        await using var stream = new FileStream(filePath, FileMode.Create);

        await fileContentStream.CopyToAsync(stream);
        
        var scheme = httpContextAccessor.HttpContext?.Request.Scheme ?? string.Empty;
        var host = httpContextAccessor.HttpContext?.Request.Host;
        var pathBase = httpContextAccessor.HttpContext?.Request.PathBase ?? string.Empty;
        
        return $"{scheme}://{host}{pathBase}/uploaded-files/{fileName}";
    }

    public Task DeleteAsync(string filePath)
    {
        var fileIndex = filePath.IndexOf("uploaded-files", StringComparison.Ordinal);

        filePath = Path.Combine(webHostEnvironment.WebRootPath, filePath[fileIndex..]);
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}