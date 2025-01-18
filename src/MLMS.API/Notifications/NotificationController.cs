using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Notifications.Responses;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Notifications;

namespace MLMS.API.Notifications;

[Authorize]
[Route("user/notifications")]
[ApiVersion("1.0")]
public class NotificationController(INotificationService notificationService) : ApiControllerBase
{
    [HttpGet("unread/count")]
    public async Task<IActionResult> CountUnread()
    {
        var result = await notificationService.CountUnreadAsync();
        
        return result.Match(count => Ok(new UnreadNotificationsCountResponse { Count = count }), Problem);
    }
    
    [HttpPatch("{id:guid}/is-read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var result = await notificationService.MarkAsReadAsync(id);

        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpPatch("is-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var result = await notificationService.MarkAllAsReadAsync();

        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Get(RetrievalRequest request)
    {
        var result = await notificationService.GetAsync(request.ToSieveModel());

        return result.Match(notifications => Ok(notifications.ToContractPaginatedList(NotificationMapper.ToContract)),
            Problem);
    }
}