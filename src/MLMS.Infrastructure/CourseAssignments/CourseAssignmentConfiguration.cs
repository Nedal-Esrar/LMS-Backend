using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MLMS.Domain.CourseAssignments;

namespace MLMS.Infrastructure.CourseAssignments;

public class CourseAssignmentConfiguration : IEntityTypeConfiguration<CourseAssignment>
{
    public void Configure(EntityTypeBuilder<CourseAssignment> builder)
    {
        builder.ToTable("CourseAssignment");
        
        builder.HasKey(ca => new { ca.CourseId, ca.MajorId });
    }
}