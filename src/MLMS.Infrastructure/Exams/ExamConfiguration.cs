using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.Exams;
using MLMS.Domain.SectionParts;

namespace MLMS.Infrastructure.Exams;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.ToTable("Exam");

        builder.Ignore(e => e.MaxGradePoints);
    }
}