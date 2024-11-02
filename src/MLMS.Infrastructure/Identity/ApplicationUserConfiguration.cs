using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.Departments;
using MLMS.Domain.Entities;
using MLMS.Domain.Majors;
using MLMS.Infrastructure.Identity.Models;

namespace MLMS.Infrastructure.Identity;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne<Major>(u => u.Major)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Department>(u => u.Department)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<RefreshToken>()
            .WithOne()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(u => u.Roles);
    }
}