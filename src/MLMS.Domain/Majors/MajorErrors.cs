using ErrorOr;

namespace MLMS.Domain.Majors;

public static class MajorErrors
{
    public static Error NotFound => Error.NotFound(
        code: "Majors.NotFound",
        description: "Major not found.");
    
    public static Error NameAlreadyExists => Error.Conflict(
        code: "Majors.NameAlreadyExists",
        description: "Another major with this name in this department already exists.");
}