using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MLMS.Domain.Email;

namespace MLMS.Infrastructure.Email;

internal static class EmailConfiguration
{
    internal static IServiceCollection ConfigureEmail(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<EmailSettings>()
            .BindConfiguration(nameof(EmailSettings));

        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}