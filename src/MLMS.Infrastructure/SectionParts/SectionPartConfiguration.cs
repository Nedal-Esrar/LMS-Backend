using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.SectionParts;

namespace MLMS.Infrastructure.SectionParts;

public class SectionPartConfiguration : IEntityTypeConfiguration<SectionPart>
{
    public void Configure(EntityTypeBuilder<SectionPart> builder)
    {
        
    }
}