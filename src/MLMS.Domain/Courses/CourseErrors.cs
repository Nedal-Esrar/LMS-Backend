using ErrorOr;
using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Courses;

public static partial class CourseErrors
{
    public static Error NameAlreadyExists => Error.Conflict("Course name already exists.");
    public static Error NotFound => Error.NotFound("Course not found.");

    public static Error NotAssigned => Error.Unauthorized("Course is not assigned for user.");

    public static Error NotCreatedByUser =>
        Error.Unauthorized("Course is not accessible since it is not created by the current user.");

    public static Error StillInProgress => Error.Conflict("Course is still in progress.");
}