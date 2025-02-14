using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.UserSectionParts;
using MLMS.Infrastructure.Identity.Models;

namespace MLMS.Infrastructure.UserSectionParts;

public class UserSectionPartDbConfiguration : IEntityTypeConfiguration<UserSectionPart>
{
    public void Configure(EntityTypeBuilder<UserSectionPart> builder)
    {
        builder.ToTable("UserSectionPart");
        
        builder.HasKey(ud => new { ud.UserId, ud.SectionPartId });
        
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}