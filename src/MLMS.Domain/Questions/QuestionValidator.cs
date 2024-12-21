using FluentValidation;

namespace MLMS.Domain.Questions;

public class QuestionValidator : AbstractValidator<Question>
{
    public QuestionValidator()
    {
        RuleFor(x => x.Text).NotEmpty().MaximumLength(500);
        
        RuleFor(x => x.Points).GreaterThan(0);
        
        RuleFor(x => x.Choices)
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.Choices)
            .NotNull()
            .SetValidator(new ChoiceValidator());
    }
}