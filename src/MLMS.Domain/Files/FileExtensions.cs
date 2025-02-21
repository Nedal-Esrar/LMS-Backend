namespace MLMS.Domain.Files;

public static class FileExtensions
{
    public static bool IsImage(this File file)
    {
        return file.ContentType switch
        {
            "image/jpeg" => true,
            "image/png" => true,
            _ => false
        };
    }
}