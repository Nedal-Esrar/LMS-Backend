using ErrorOr;

namespace MLMS.Domain.Sections;

public static class SectionErrors
{
    public static Error NotFound => Error.NotFound("Section not found", "Section not found");
    public static Error NameAlreadyExists => Error.Conflict("Section name already exists", "Section name already exists");
}