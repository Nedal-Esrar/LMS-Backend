using ErrorOr;

namespace MLMS.Domain.Courses;

public static class CourseErrors
{
    public static Error NameAlreadyExists => Error.Conflict(
        code: "Courses.NameAlreadyExists",
        description: "A course with the same name already exists.");
    
    public static Error NotFound => Error.NotFound(
        code: "Courses.NotFound",
        description: "Course not found.");

    public static Error NotAssigned => Error.Forbidden(
        code: "Courses.NotAssigned",
        description: "This course is not assigned for user.");

    public static Error NotCreatedByUser =>
        Error.Forbidden(
            code: "Courses.NotCreatedByUser",
            description: "Course is not accessible since it is not created by the current user.");
    
    public static Error NotInProgress => Error.Conflict(
        code: "Courses.NotInProgress",
        description: "Course is not in progress.");

    public static Error StillInProgress => Error.Conflict(
        code: "Courses.StillInProgress",
        description: "Course is still in progress.");
    
    public static Error AlreadyStarted => Error.Conflict(
        code: "Courses.AlreadyStarted",
        description: "Course is already started.");
}