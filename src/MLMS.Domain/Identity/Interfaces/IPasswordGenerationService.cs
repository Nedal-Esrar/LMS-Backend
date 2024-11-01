using ErrorOr;

namespace MLMS.Domain.Identity;

public interface IPasswordGenerationService
{
    ErrorOr<string> GenerateStrongPassword(int length);
}