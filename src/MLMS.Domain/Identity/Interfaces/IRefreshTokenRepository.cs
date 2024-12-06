using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Identity.Interfaces;

public interface IRefreshTokenRepository
{
    Task CreateAsync(RefreshToken refreshToken);
    
    Task<RefreshToken?> GetByTokenAsync(string refreshToken);
    
    Task DeleteAsync(Guid id);

    Task DeleteAsync(string token);
    
    Task UpdateAsync(RefreshToken refreshTokenModel);
    
    Task<bool> ExistsByTokenAsync(string refreshToken);
}