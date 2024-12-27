using ErrorOr;
using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Courses;

public static partial class CourseErrors
{
    public static Error NameAlreadyExists => Error.Conflict("Course name already exists.");
    public static Error NotFound => Error.NotFound("Course not found.");

    public static Error NotAssigned => Error.Forbidden("Course is not assigned for user.");

    public static Error NotCreatedByUser =>
        Error.Forbidden("Course is not accessible since it is not created by the current user.");
    
    public static Error NotInProgress => Error.Conflict("Course is not in progress.");

    public static Error StillInProgress => Error.Conflict("Course is still in progress.");
    
    public static Error AlreadyStarted => Error.Conflict("Course is already started.");
}