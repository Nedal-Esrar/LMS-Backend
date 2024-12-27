using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.Exams;

namespace MLMS.Infrastructure.Exams;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Question");
        
        builder.HasOne(x => x.Image)
            .WithMany()
            .HasForeignKey(x => x.ImageId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}