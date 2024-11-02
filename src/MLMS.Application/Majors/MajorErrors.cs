using ErrorOr;

namespace MLMS.Application.Majors;

public static class MajorErrors
{
    public static Error NotFound => Error.NotFound("Major not found.");
    
    public static Error NameAlreadyExists => Error.Conflict("Major name already exists.");
}