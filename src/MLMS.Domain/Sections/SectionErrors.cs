using ErrorOr;

namespace MLMS.Domain.Sections;

public static class SectionErrors
{
    public static Error NotFound => Error.NotFound(
        code: "Sections.NotFound",
        description: "Section not found");
    
    public static Error NameAlreadyExists => Error.Conflict(
        code: "Sections.NameAlreadyExists",
        description: "Another section with the same name in this course already exists");
}