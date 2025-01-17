using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;

namespace MLMS.Infrastructure.Email;

public class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    
    public async Task SendAsync(EmailRequest request)
    {
        var email = CreateEmail(request);
        
        using var smtpClient = new SmtpClient();
        
        try
        {
            await smtpClient.ConnectAsync(
                _emailSettings.Server,
                _emailSettings.Port,
                SecureSocketOptions.StartTls);

            await smtpClient.AuthenticateAsync(
                _emailSettings.Username,
                _emailSettings.Password);
      
            await smtpClient.SendAsync(email);
        }
        finally
        {
            await smtpClient.DisconnectAsync(quit: true);
        }
    }
    
    private MimeMessage CreateEmail(EmailRequest emailRequest)
    {
        var emailMessage = new MimeMessage();
        
        emailMessage.From.Add(new MailboxAddress(_emailSettings.FromEmail, _emailSettings.FromEmail));

        emailMessage.To.AddRange(emailRequest.ToEmails.Select(MailboxAddress.Parse));

        emailMessage.Subject = emailRequest.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = emailRequest.BodyHtml
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();

        return emailMessage;
    }
}