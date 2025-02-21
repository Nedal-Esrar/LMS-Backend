using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.Exams;

namespace MLMS.Infrastructure.Exams;

public class ExamSessionQuestionChoiceDbConfiguration : IEntityTypeConfiguration<ExamSessionQuestionChoice>
{
    public void Configure(EntityTypeBuilder<ExamSessionQuestionChoice> builder)
    {
        builder.ToTable("ExamSessionQuestionChoice");

        builder.HasKey(s => new { s.ExamSessionId, s.QuestionId });
    }
}