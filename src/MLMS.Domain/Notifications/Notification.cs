using MLMS.Domain.Common.Models;
using MLMS.Domain.Users;

namespace MLMS.Domain.Notifications;

public class Notification : EntityBase<Guid>
{
    public int UserId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
    
    public string? Link { get; set; }
    
    public bool IsRead { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
}