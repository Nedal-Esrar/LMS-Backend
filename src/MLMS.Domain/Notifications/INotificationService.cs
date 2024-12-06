using ErrorOr;
using MLMS.Domain.Common.Models;
using Sieve.Models;

namespace MLMS.Domain.Notifications;

public interface INotificationService
{
    Task<ErrorOr<int>> CountUnreadAsync();
    
    Task<ErrorOr<None>> MarkAsReadAsync(Guid id);
    
    Task<ErrorOr<PaginatedList<Notification>>> GetAsync(SieveModel sieveModel);
}