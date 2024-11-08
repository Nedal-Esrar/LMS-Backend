using ErrorOr;

namespace MLMS.Domain.Majors;

public static class MajorErrors
{
    public static Error NotFound => Error.NotFound("Major not found.");
    
    public static Error NameAlreadyExists => Error.Conflict("Major name already exists.");
}