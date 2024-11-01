using FluentValidation;
using MLMS.Domain.Majors;

namespace MLMS.Application.Majors;

public class MajorValidator : AbstractValidator<Major>
{
    public MajorValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}