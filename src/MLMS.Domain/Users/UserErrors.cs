using ErrorOr;
using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Users;

public static class UserErrors
{
    public static Error NotFound => Error.NotFound(
        code: "Users.UserNotFound",
        description: "User not found.");
    
    public static Error WorkIdExists => Error.Conflict(
        code: "Users.WorkIdExists",
        description: "Another User with the same WorkId already exists.");

    public static Error NotSubAdmin => Error.Validation(
        code: "Users.NotSubAdmin",
        description: "User is not a SubAdmin.");
}