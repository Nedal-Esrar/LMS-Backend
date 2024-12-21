using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.UserSectionParts;

namespace MLMS.Infrastructure.UserSectionParts;

public class UserExamStateForSectionPartConfiguration : IEntityTypeConfiguration<UserExamStateForSectionPart>
{
    public void Configure(EntityTypeBuilder<UserExamStateForSectionPart> builder)
    {
        builder.HasKey(ue => new { ue.UserId, ue.SectionPartId });
    }
}