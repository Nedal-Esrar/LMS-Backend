using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.UserSectionParts;

namespace MLMS.Infrastructure.UserSectionParts;

public class UserDoneStateForSectionPartConfiguration : IEntityTypeConfiguration<UserDoneStateForSectionPart>
{
    public void Configure(EntityTypeBuilder<UserDoneStateForSectionPart> builder)
    {
        builder.HasKey(ud => new { ud.UserId, ud.SectionPartId });
    }
}