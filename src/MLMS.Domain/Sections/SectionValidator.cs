using FluentValidation;

namespace MLMS.Domain.Sections;

public class SectionValidator : AbstractValidator<Section>
{
    public SectionValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(150);
    }
}