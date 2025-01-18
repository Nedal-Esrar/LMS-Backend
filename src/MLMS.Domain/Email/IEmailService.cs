namespace MLMS.Domain.Email;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
}