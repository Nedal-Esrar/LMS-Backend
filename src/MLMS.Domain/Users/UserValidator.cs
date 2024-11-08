using FluentValidation;
using MLMS.Domain.Common;
using MLMS.Domain.Identity;

namespace MLMS.Domain.Users;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
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
    }
}