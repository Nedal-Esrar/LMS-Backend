using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MLMS.Infrastructure.CourseAssignments;
using MLMS.Infrastructure.Courses;
using MLMS.Infrastructure.Departments;
using MLMS.Infrastructure.Exams;
using MLMS.Infrastructure.Files;
using MLMS.Infrastructure.Identity;
using MLMS.Infrastructure.Identity.Models;
using MLMS.Infrastructure.Majors;
using MLMS.Infrastructure.Notifications;
using MLMS.Infrastructure.SectionParts;
using MLMS.Infrastructure.Sections;
using MLMS.Infrastructure.UserCourses;
using MLMS.Infrastructure.UserSectionParts;

namespace MLMS.Infrastructure.Common;

public static class PersistenceExtensions
{
    internal static ModelBuilder ApplyLmsDbConfiguration(this ModelBuilder builder)
    {
        return builder.ApplyConfiguration(new DepartmentDbConfiguration())
            .ApplyConfiguration(new MajorDbConfiguration())
            .ApplyConfiguration(new FileDbConfiguration())
            .ApplyConfiguration(new CourseAssignmentDbConfiguration())
            .ApplyConfiguration(new CourseDbConfiguration())
            .ApplyConfiguration(new ChoiceDbConfiguration())
            .ApplyConfiguration(new QuestionDbConfiguration())
            .ApplyConfiguration(new SectionDbConfiguration())
            .ApplyConfiguration(new SectionPartDbConfiguration())
            .ApplyConfiguration(new UserCourseDbConfiguration())
            .ApplyConfiguration(new UserSectionPartDoneDbConfiguration())
            .ApplyConfiguration(new UserExamStateDbConfiguration())
            .ApplyConfiguration(new ExamDbConfiguration())
            .ApplyConfiguration(new ExamSessionDbConfiguration())
            .ApplyConfiguration(new ExamSessionQuestionChoiceDbConfiguration())
            .ApplyConfiguration(new RefreshTokenDbConfiguration())
            .ApplyConfiguration(new NotificationDbConfiguration());
    }
    
    internal static ModelBuilder ConfigureIdentity(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserDbConfiguration())
            .ApplyConfiguration(new RoleDbConfiguration());
        
        builder.Ignore<IdentityUserToken<int>>();
        builder.Ignore<IdentityUserRole<int>>();
        builder.Ignore<IdentityUserLogin<int>>();
        builder.Ignore<IdentityUserClaim<int>>();
        builder.Ignore<IdentityRoleClaim<int>>();

        return builder;
    }
}