using MLMS.Domain.Entities;

namespace MLMS.Domain.Common.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
}