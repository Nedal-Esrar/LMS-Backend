using FluentValidation;

namespace MLMS.Domain.Majors;

public class MajorValidator : AbstractValidator<Major>
{
    public MajorValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}