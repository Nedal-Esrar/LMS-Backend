using System.Text.Json.Serialization;
using MLMS.API.Common;
using MLMS.API.Common.Middlewares;
using MLMS.API.Identity;
using MLMS.Domain.Enums;
using MLMS.Domain.Identity.Interfaces;

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
        services.AddSwaggerGen();

        services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationPolicies.SuperAdmin, policy => policy.RequireRole(UserRole.Admin.ToString()));

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        
        services.AddCors(c =>
        {
            c.AddPolicy("AllowOrigin",
                options =>
                    options
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });

        return services;
    }
}