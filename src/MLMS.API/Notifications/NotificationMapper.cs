using MLMS.API.Notifications.Responses;
using MLMS.Domain.Notifications;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.Notifications;

[Mapper]
public static partial class NotificationMapper
{
    public static partial NotificationResponse ToContract(this Notification notification);
}