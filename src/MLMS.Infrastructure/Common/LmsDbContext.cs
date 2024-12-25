using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Courses;
using MLMS.Domain.Departments;
using MLMS.Domain.Exams;
using MLMS.Domain.Majors;
using MLMS.Domain.Notifications;
using MLMS.Domain.SectionParts;
using MLMS.Domain.Sections;
using MLMS.Domain.UsersCourses;
using MLMS.Domain.UserSectionParts;
using MLMS.Infrastructure.CourseAssignments;
using MLMS.Infrastructure.Courses;
using MLMS.Infrastructure.Departments;
using MLMS.Infrastructure.Files;
using MLMS.Infrastructure.Identity;
using MLMS.Infrastructure.Identity.Models;
using MLMS.Infrastructure.Majors;
using MLMS.Infrastructure.Questions;
using MLMS.Infrastructure.SectionParts;
using MLMS.Infrastructure.Sections;
using MLMS.Infrastructure.UserCourses;
using MLMS.Infrastructure.UserSectionParts;
using File = MLMS.Domain.Files.File;

namespace MLMS.Infrastructure.Common;

public class LmsDbContext(DbContextOptions options)
    : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options)
{
    public DbSet<Department> Departments { get; set; }
    
    public DbSet<Major> Majors { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public DbSet<File> Files { get; set; }
    
    public DbSet<Notification> Notifications { get; set; }
    
    public DbSet<CourseAssignment> CourseAssignments { get; set; }
    
    public DbSet<Course> Courses { get; set; }
    
    public DbSet<Choice> Choices { get; set; }
    
    public DbSet<Question> Questions { get; set; }
    
    public DbSet<Section> Sections { get; set; }
    
    public DbSet<SectionPart> SectionParts { get; set; }
    
    public DbSet<UserCourse> UserCourses { get; set; }
    
    public DbSet<UserSectionPartDone> UserSectionPartDoneRelations { get; set; }
    
    public DbSet<Domain.UserSectionParts.UserSectionPartExamState> UserSectionPartExamStateRelations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new DepartmentConfiguration());
        builder.ApplyConfiguration(new MajorConfiguration());
        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new FileConfiguration());
        builder.ApplyConfiguration(new CourseAssignmentConfiguration());
        builder.ApplyConfiguration(new CourseConfiguration());
        builder.ApplyConfiguration(new ChoiceConfiguration());
        builder.ApplyConfiguration(new QuestionConfiguration());
        builder.ApplyConfiguration(new SectionConfiguration());
        builder.ApplyConfiguration(new SectionPartConfiguration());
        builder.ApplyConfiguration(new UserCourseConfiguration());
        builder.ApplyConfiguration(new UserSectionPartDoneConfiguration());
        builder.ApplyConfiguration(new UserSectionPartExamStateConfiguration());
    }
}