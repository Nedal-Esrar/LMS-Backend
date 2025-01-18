using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Courses;
using MLMS.Domain.Departments;
using MLMS.Domain.Exams;
using MLMS.Domain.Files;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Majors;
using MLMS.Domain.Notifications;
using MLMS.Domain.SectionParts;
using MLMS.Domain.Sections;
using MLMS.Domain.UsersCourses;
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

namespace MLMS.Infrastructure.Persistence;

internal static class PersistenceConfiguration
{
    internal static IServiceCollection ConfigurePersistence(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        // Configure Identity.
        services.AddIdentity<ApplicationUser, IdentityRole<int>>(options => 
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.User.RequireUniqueEmail = false;
        })
        .AddEntityFrameworkStores<LmsDbContext>()
        .AddDefaultTokenProviders();
        
        // Configure DbContext.
        services.AddDbContext<LmsDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("LmsDb"));
            
            if (environment.IsDevelopment())
            {
                options.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging();
            }
        });

        // Register Repositories.
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMajorRepository, MajorRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ICourseAssignmentRepository, CourseAssignmentRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ISectionPartRepository, SectionPartRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<IUserCourseRepository, UserCourseRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();
        
        services.AddScoped<IDbTransactionProvider, DbTransactionProvider>();
        services.AddScoped<IFileHandler, FileHandler>();
        
        return services;
    }
}