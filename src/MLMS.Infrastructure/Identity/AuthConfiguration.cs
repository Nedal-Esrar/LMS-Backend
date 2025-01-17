using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MLMS.Domain.Identity.Interfaces;

namespace MLMS.Infrastructure.Identity;

internal static class AuthConfiguration
{
    internal static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AuthSettings>()
            .BindConfiguration(nameof(AuthSettings));
        
        using var scope = services.BuildServiceProvider().CreateScope();
        var authSettings = scope.ServiceProvider
            .GetRequiredService<IOptions<AuthSettings>>().Value;

        services.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromMinutes(authSettings.ResetPasswordTokenLifeTimeMinutes));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(authSettings.Secret);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authSettings.Issuer,
                ValidAudience = authSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }   
}