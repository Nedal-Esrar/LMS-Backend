using ErrorOr;

namespace MLMS.Domain.Identity;

public static class IdentityErrors
{
    public static Error InvalidCredentials => Error.Unauthorized(
        code: "Identity.InvalidCredentials",
        description: "Invalid credentials provided.");
    
    public static Error WeakPassword => Error.Validation(
        code: "Identity.WeakPassword",
        description: "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit and one special character.");
    
    public static Error InvalidRefreshToken => Error.Unauthorized(
        code: "Identity.InvalidRefreshToken",
        description: "Invalid refresh token provided.");
    
    public static Error WrongCurrentPassword => Error.Conflict(
        code: "Identity.WrongCurrentPassword",
        description: "Current password is incorrect.");
    
    public static Error InvalidResetPasswordToken => Error.Unauthorized(
        code: "Identity.InvalidResetPasswordToken",
        description: "Invalid reset password token provided.");
}