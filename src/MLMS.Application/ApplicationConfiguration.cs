using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MLMS.Application.Departments;
using MLMS.Application.Identity;
using MLMS.Application.Majors;
using MLMS.Domain.Departments;
using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Majors;

namespace MLMS.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IMajorService, MajorService>();
        services.AddScoped<IPasswordGenerationService, PasswordGenerationService>();

        services.AddOptions<ClientOptions>()
            .BindConfiguration(nameof(ClientOptions));
        
        return services;
    }
}