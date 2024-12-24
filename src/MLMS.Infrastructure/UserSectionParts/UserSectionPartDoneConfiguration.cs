using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.UserSectionParts;
using MLMS.Infrastructure.Identity.Models;

namespace MLMS.Infrastructure.UserSectionParts;

public class UserSectionPartDoneConfiguration : IEntityTypeConfiguration<UserSectionPartDone>
{
    public void Configure(EntityTypeBuilder<UserSectionPartDone> builder)
    {
        builder.ToTable("UserSectionPartDone");
        
        builder.HasKey(ud => new { ud.UserId, ud.SectionPartId });
        
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}