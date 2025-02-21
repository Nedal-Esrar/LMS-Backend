using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Courses;
using MLMS.Domain.Departments;
using MLMS.Domain.Exams;
using MLMS.Domain.ExamSessions;
using MLMS.Domain.Identity.Models;
using MLMS.Domain.Majors;
using MLMS.Domain.Notifications;
using MLMS.Domain.SectionParts;
using MLMS.Domain.Sections;
using MLMS.Domain.UsersCourses;
using MLMS.Domain.UserSectionParts;
using MLMS.Infrastructure.Identity.Models;
using File = MLMS.Domain.Files.File;

namespace MLMS.Infrastructure.Persistence;

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
    
    public DbSet<UserSectionPart> UserSectionPartRelations { get; set; }
    
    public DbSet<Exam> Exams { get; set; }
    
    public DbSet<UserExamState> UserExamStateRelations { get; set; }
    
    public DbSet<ExamSession> ExamSessions { get; set; }
    
    public DbSet<ExamSessionQuestionChoice> ExamSessionQuestionChoices { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureEntities();
    }
}