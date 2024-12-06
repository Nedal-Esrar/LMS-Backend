namespace MLMS.API.Notifications.Responses;

public class NotificationResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
    
    public string? Link { get; set; }
    
    public bool IsRead { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
}