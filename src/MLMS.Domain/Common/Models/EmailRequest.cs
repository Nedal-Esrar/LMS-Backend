namespace MLMS.Domain.Entities;

public class EmailRequest
{
    public List<string> ToEmails { get; set; } = [];
    
    public string Subject { get; set; }

    public string Body { get; set; }

    public List<Attachment> Attachments { get; set; } = [];
}