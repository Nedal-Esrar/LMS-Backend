using FluentValidation;
using MLMS.Domain.Common;

namespace MLMS.Domain.Identity.Validators;

public class LoginValidator : AbstractValidator<LoginCredentials>
{
    public LoginValidator()
    {
        RuleFor(x => x.WorkId)
            .NotEmpty();

        // RuleFor(x => x.Password)
        //     .NotEmpty()
        //     .StrongPassword();
    }
}