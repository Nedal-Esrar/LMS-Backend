using ErrorOr;

namespace MLMS.Application.Majors;

public static class MajorErrors
{
    public static Error NotFound => Error.NotFound("Major not found.");
}