using ErrorOr;

namespace MLMS.Domain.Departments;

public static class DepartmentErrors
{
    public static Error NotFound => Error.NotFound(
        code: "Departments.NotFound",
        description: "Department not found.");
    
    public static Error NameAlreadyExists => Error.Conflict(
        code: "Departments.NameAlreadyExists",
        description: "Another Department with the same name already exists.");
}