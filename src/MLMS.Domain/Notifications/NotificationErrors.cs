using ErrorOr;

namespace MLMS.Domain.Notifications;

public static class NotificationErrors
{
    public static Error NotFound => Error.NotFound(
        code: "Notifications.NotFound",
        description: "Notification not found.");
}