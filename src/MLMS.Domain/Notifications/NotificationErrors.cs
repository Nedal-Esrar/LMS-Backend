using ErrorOr;

namespace MLMS.Domain.Notifications;

public static class NotificationErrors
{
    public static Error NotFound => Error.NotFound("Notification not found.");
}