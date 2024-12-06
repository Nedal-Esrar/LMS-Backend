using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Notifications;
using MLMS.Infrastructure.Common;
using Sieve.Models;
using Sieve.Services;

namespace MLMS.Infrastructure.Notifications;

public class NotificationRepository(
    LmsDbContext context,
    ISieveProcessor sieveProcessor) : INotificationRepository
{
    public async Task<int> CountUnreadForUserAsync(int userId)
    {
        return await context.Notifications.CountAsync(n => n.UserId == userId);
    }

    public async Task<bool> ExistsAsync(int userId, Guid id)
    {
        return await context.Notifications.AnyAsync(n => n.UserId == userId && n.Id == id);
    }

    public async Task MarkAsReadAsync(Guid id)
    {
        await context.Notifications.Where(n => n.Id == id)
            .ExecuteUpdateAsync(spc => spc.SetProperty(n => n.IsRead, true));
    }

    public async Task<PaginatedList<Notification>> GetForUserAsync(int userId, SieveModel sieveModel)
    {
        var query = context.Notifications.Where(n => n.UserId == userId)
            .AsQueryable();

        var totalItems = await sieveProcessor.Apply(sieveModel, query, applyPagination: false).CountAsync();

        query = sieveProcessor.Apply(sieveModel, query);

        var result = await query.AsNoTracking()
            .ToListAsync();

        return new PaginatedList<Notification>
        {
            Items = result,
            Metadata = new PaginationMetadata
            {
                Page = sieveModel.Page!.Value,
                PageSize = sieveModel.PageSize!.Value,
                TotalItems = totalItems
            }
        };
    }
}