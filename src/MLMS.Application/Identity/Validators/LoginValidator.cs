using System.Data;
using FluentValidation;
using MLMS.Application.Common;
using MLMS.Domain.Models;

namespace MLMS.Application.Identity.Validators;

public class LoginValidator : AbstractValidator<LoginCredentials>
{
    public LoginValidator()
    {
        RuleFor(x => x.WorkId)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .StrongPassword();
    }
}