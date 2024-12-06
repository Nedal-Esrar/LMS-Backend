using FluentValidation;
using MLMS.Domain.Common;
using MLMS.Domain.Users;

namespace MLMS.Domain.Identity.Validators;

public class RegisterValidator : AbstractValidator<User>
{
    public RegisterValidator()
    {
        RuleFor(x => x.WorkId)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(x => x.MiddleName)
            .NotEmpty()
            .MaximumLength(100);
        
        RuleFor(x => x.LastName)
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

        RuleFor(x => x.Role)
            .Must(x => x is UserRole.SubAdmin or UserRole.Staff)
            .WithMessage("Role must be SubAdmin or Staff");
    }
}