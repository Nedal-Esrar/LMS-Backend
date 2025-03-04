using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.SectionParts;

namespace MLMS.Infrastructure.SectionParts;

public class SectionPartDbConfiguration : IEntityTypeConfiguration<SectionPart>
{
    public void Configure(EntityTypeBuilder<SectionPart> builder)
    {
        builder.ToTable("SectionPart");
        
        builder.HasOne(x => x.File)
            .WithMany()
            .HasForeignKey(x => x.FileId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Exam)
            .WithOne()
            .HasForeignKey<SectionPart>(x => x.ExamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}