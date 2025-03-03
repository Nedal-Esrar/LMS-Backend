namespace MLMS.Domain.Files;

public static class FileExtensions
{
    public static bool IsImage(this File file) => file.ContentType is "image/jpeg" or "image/png";
}