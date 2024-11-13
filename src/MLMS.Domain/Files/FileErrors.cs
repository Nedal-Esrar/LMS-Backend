using ErrorOr;

namespace MLMS.Domain.Files;

public static class FileErrors
{
    public static Error NotFound => Error.NotFound("File.NotFound", "File not found.");
    public static Error NotImage => Error.Validation("File.NotImage", "File is not an image.");
}