using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.Exams;
using MLMS.Infrastructure.Identity.Models;

namespace MLMS.Infrastructure.Exams;

public class UserExamStateDbConfiguration : IEntityTypeConfiguration<UserExamState>
{
    public void Configure(EntityTypeBuilder<UserExamState> builder)
    {
        builder.ToTable("UserExamState");
        
        builder.HasKey(ue => new { ue.UserId, ue.ExamId });

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}