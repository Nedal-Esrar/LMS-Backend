using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Common.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
}