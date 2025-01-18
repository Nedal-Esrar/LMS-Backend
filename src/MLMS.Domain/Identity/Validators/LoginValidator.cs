using FluentValidation;
using MLMS.Domain.Identity.Models;

namespace MLMS.Domain.Identity.Validators;

public class LoginValidator : AbstractValidator<LoginCredentials>
{
    public LoginValidator()
    {
        RuleFor(x => x.WorkId)
            .NotEmpty();

        // TODO: evaluate this if needed or not.
        // RuleFor(x => x.Password)
        //     .NotEmpty()
        //     .StrongPassword();
    }
}