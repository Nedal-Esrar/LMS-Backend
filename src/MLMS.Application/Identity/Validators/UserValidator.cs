using FluentValidation;
using MLMS.Application.Common;
using MLMS.Domain.Entities;
using MLMS.Domain.Enums;

namespace MLMS.Application.Identity.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.WorkId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .PhoneNumber();

        RuleFor(x => x.Gender)
            .NotEmpty()
            .IsInEnum();

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow));

        RuleFor(x => x.EducationalLevel)
            .NotEmpty()
            .IsInEnum();

        RuleForEach(x => x.Roles)
            .Must(x => x is UserRole.SubAdmin or UserRole.Staff)
            .WithMessage("Role must be SubAdmin or Staff");
    }
}