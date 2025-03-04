using FluentValidation;

namespace MLMS.Domain.Exams;

public class ChoiceValidator : AbstractValidator<Choice>
{
    public ChoiceValidator()
    {
        RuleFor(x => x.Text).NotEmpty().MaximumLength(150);
    }
}