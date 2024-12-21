using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MLMS.Domain.Courses;
using MLMS.Domain.Departments;
using MLMS.Domain.Files;
using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Majors;
using MLMS.Domain.Notifications;
using MLMS.Domain.SectionParts;
using MLMS.Domain.Sections;
using MLMS.Domain.Users;

namespace MLMS.Domain;

public static class DomainConfiguration
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IMajorService, MajorService>();
        services.AddScoped<IPasswordGenerationService, PasswordGenerationService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ISectionPartService, SectionPartService>();
        services.AddScoped<ISectionService, SectionService>();

        services.AddOptions<ClientOptions>()
            .BindConfiguration(nameof(ClientOptions));
        
        return services;
    }
}