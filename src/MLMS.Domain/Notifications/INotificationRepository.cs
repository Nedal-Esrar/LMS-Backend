using ErrorOr;
using MLMS.Domain.Common.Models;
using Sieve.Models;

namespace MLMS.Domain.Notifications;

public interface INotificationRepository
{
    Task<int> CountUnreadForUserAsync(int userId);
    
    Task<bool> ExistsAsync(int userId, Guid id);
    
    Task MarkAsReadAsync(Guid id);
    
    Task<PaginatedList<Notification>> GetForUserAsync(int userId, SieveModel sieveModel);
}