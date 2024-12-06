using MLMS.Domain.Notifications;
using Sieve.Services;

namespace MLMS.Infrastructure.Notifications;

public class NotificationSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Notification>(n => n.IsRead)
            .CanFilter();

        mapper.Property<Notification>(n => n.CreatedAtUtc)
            .CanSort();
    }
}