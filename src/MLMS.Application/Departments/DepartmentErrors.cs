using ErrorOr;

namespace MLMS.Application.Departments;

public static partial class DepartmentErrors
{
    public static Error NotFound => Error.NotFound("Department not found.");
    
    public static Error NameAlreadyExists => Error.Conflict("Department name already exists.");
}