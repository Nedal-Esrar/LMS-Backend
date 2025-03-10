using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.Courses;
using MLMS.Infrastructure.Identity.Models;

namespace MLMS.Infrastructure.Courses;

public class CourseDbConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Course");

        builder.HasQueryFilter(c => !c.IsDeleted);
        
        builder.HasKey(x => x.Id);
        
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Ignore(x => x.CreatedBy);

        builder.Ignore(x => x.Progress);
    }
}