using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.Notifications;

namespace MLMS.Infrastructure.Notifications;

public class NotificationDbConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notification");
        
        builder.HasKey(n => n.Id);
    }
}