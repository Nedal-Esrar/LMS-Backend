using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Infrastructure.Common;

namespace MLMS.Infrastructure.Identity;

public class RefreshTokenRepository(LmsDbContext context) : IRefreshTokenRepository
{
    public async Task CreateAsync(RefreshToken refreshToken)
    {
        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByTokenAsync(string refreshToken)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
    }

    public async Task DeleteAsync(Guid id)
    {
        var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == id);

        if (refreshToken is null)
        {
            return;
        }

        context.RefreshTokens.Remove(refreshToken);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string token)
    {
        var refreshToken = context.RefreshTokens.FirstOrDefault(rt => rt.Token == token);

        if (refreshToken is null)
        {
            return;
        }

        context.RefreshTokens.Remove(refreshToken);

        await  context.SaveChangesAsync();
    }

    public Task UpdateAsync(RefreshToken refreshTokenModel)
    {
        context.RefreshTokens.Update(refreshTokenModel);

        return context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByTokenAsync(string refreshToken)
    {
        return await context.RefreshTokens.AnyAsync(rt => rt.Token == refreshToken);
    }
}