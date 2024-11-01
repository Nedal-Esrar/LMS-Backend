using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MLMS.Domain.Entities;
using MLMS.Domain.Identity.Interfaces;

namespace MLMS.Infrastructure.Identity;

public class TokenGenerator(IOptions<AuthSettings> authSettings) : ITokenGenerator
{
    private readonly AuthSettings _authSettings = authSettings.Value;
    
    public AccessToken GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Expiration, DateTime.UtcNow.AddMinutes(_authSettings.AccessTokenLifeTimeMinutes).ToString(CultureInfo.InvariantCulture))
        };

        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.ToString())));

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Secret)),
            SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            _authSettings.Issuer,
            _authSettings.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_authSettings.AccessTokenLifeTimeMinutes),
            signingCredentials
        );

        var token = new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);

        return new AccessToken { Token = token };
    }

    public RefreshToken GenerateRefreshToken(int userId)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = Guid.NewGuid().ToString(),
            Expiration = DateTime.UtcNow.AddDays(_authSettings.RefreshTokenLifeTimeDays)
        };

        return refreshToken;
    }
}