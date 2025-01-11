using ErrorOr;

namespace MLMS.Domain.Users;

public static class UserErrors
{
    public static Error NotFound => Error.NotFound(
        code: "Users.UserNotFound",
        description: "User not found.");
    
    public static Error WorkIdExists => Error.Conflict(
        code: "Users.WorkIdExists",
        description: "Another User with the same WorkId already exists."); 
}