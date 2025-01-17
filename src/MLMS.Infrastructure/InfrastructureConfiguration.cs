using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
using MLMS.Infrastructure.Email;
using MLMS.Infrastructure.Exams;
using MLMS.Infrastructure.Files;
using MLMS.Infrastructure.Identity;
using MLMS.Infrastructure.Identity.Models;
using MLMS.Infrastructure.Majors;
using MLMS.Infrastructure.Notifications;
using MLMS.Infrastructure.Persistence;
using MLMS.Infrastructure.SectionParts;
using MLMS.Infrastructure.Sections;
using MLMS.Infrastructure.UserCourses;
using Sieve.Models;
using Sieve.Services;

namespace MLMS.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.ConfigurePersistence(configuration, environment)
            .ConfigureAuth(configuration)
            .ConfigureEmail(configuration)
            .ConfigureSieve(configuration)
            .ConfigureCoursesExpiryJob();
        
        return services;
    }
    
    private static IServiceCollection ConfigureSieve(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SieveOptions>()
            .BindConfiguration(nameof(SieveOptions));
        
        services.AddScoped<ISieveProcessor, LmsSieveProcessor>();
        
        return services;
    }
}