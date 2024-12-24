using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.UserSectionParts;
using MLMS.Infrastructure.Identity.Models;

namespace MLMS.Infrastructure.UserSectionParts;

public class UserSectionPartExamStateConfiguration : IEntityTypeConfiguration<UserSectionPartExamState>
{
    public void Configure(EntityTypeBuilder<UserSectionPartExamState> builder)
    {
        builder.ToTable("UserSectionPartExamState");
        
        builder.HasKey(ue => new { ue.UserId, ue.SectionPartId });

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}