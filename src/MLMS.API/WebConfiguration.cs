using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using MLMS.API.Common;
using MLMS.API.Common.Middlewares;
using MLMS.API.Identity;
using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Interfaces;

using static MLMS.API.Common.AuthorizationPolicies;

namespace MLMS.API;

public static class WebConfiguration
{
    public static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        
        services.AddProblemDetails()
            .AddExceptionHandler<GlobalExceptionHandler>();
        
        services.AddEndpointsApiExplorer();
        services.ConfigureSwagger();

        services.AddAuthorizationPolicies();
        
        services.AddDateOnlyTimeOnlyStringConverters();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.ConfigureCors();

        return services;
    }
    
    private static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1", new OpenApiInfo { Title = "LMS API", Version = "v1" });

            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

            if (File.Exists(xmlCommentsFullPath))
            {
                setup.IncludeXmlComments(xmlCommentsFullPath);
            }

            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            
            setup.UseDateOnlyTimeOnlyStringConverters();
        });

        return services;
    }

    private static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(SuperAdmin, policy => policy.RequireRole(UserRole.Admin.ToString()))
            .AddPolicy(Staff, policy => policy.RequireRole(UserRole.Staff.ToString()))
            .AddPolicy(Admin, policy => policy.RequireRole(UserRole.Admin.ToString(), UserRole.SubAdmin.ToString()));

        return services;
    }
    
    private static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(c =>
        {
            c.AddDefaultPolicy(options =>
                options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
        
        return services;
    }
}