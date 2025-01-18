using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Departments;
using MLMS.Domain.Identity.Models;
using MLMS.Domain.Majors;
using MLMS.Domain.Notifications;
using MLMS.Infrastructure.Identity.Models;

namespace MLMS.Infrastructure.Identity;

public class UserDbConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("User");
        
        builder.HasOne<Major>(u => u.Major)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne<Department>(u => u.Department)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany<RefreshToken>(u => u.RefreshTokens)
            .WithOne()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<Notification>()
            .WithOne()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<IdentityRole<int>>(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.ProfilePicture)
            .WithOne()
            .HasForeignKey<ApplicationUser>(u => u.ProfilePictureId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Ignore(u => u.EmailConfirmed)
            .Ignore(u => u.ConcurrencyStamp)
            .Ignore(u => u.TwoFactorEnabled)
            .Ignore(u => u.LockoutEnd)
            .Ignore(u => u.LockoutEnabled)
            .Ignore(u => u.AccessFailedCount);
    }
}