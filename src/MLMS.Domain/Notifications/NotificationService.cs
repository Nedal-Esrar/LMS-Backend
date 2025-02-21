using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Identity.Interfaces;
using Sieve.Models;

namespace MLMS.Domain.Notifications;

public class NotificationService(
    IUserContext userContext,
    INotificationRepository notificationRepository) : INotificationService
{
    public async Task<ErrorOr<int>> CountUnreadAsync()
    {
        return await notificationRepository.CountUnreadForUserAsync(userContext.Id);
    }

    public async Task<ErrorOr<None>> MarkAsReadAsync(Guid id)
    {
        if (!await notificationRepository.ExistsAsync(userContext.Id, id))
        {
            return NotificationErrors.NotFound;
        }
        
        await notificationRepository.MarkAsReadAsync(id);

        return None.Value;
    }

    public async Task<ErrorOr<PaginatedList<Notification>>> GetAsync(SieveModel sieveModel)
    {
        return await notificationRepository.GetForUserAsync(userContext.Id, sieveModel);
    }

    public async Task<ErrorOr<None>> MarkAllAsReadAsync()
    {
        await notificationRepository.MarkAllAsReadAsync(userContext.Id);

        return None.Value;
    }
}