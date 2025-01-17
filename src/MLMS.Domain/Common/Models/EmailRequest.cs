namespace MLMS.Domain.Common.Models;

public class EmailRequest
{
    public List<string> ToEmails { get; set; } = [];

    public string Subject { get; set; } = string.Empty;

    public string BodyHtml { get; set; } = string.Empty;
}