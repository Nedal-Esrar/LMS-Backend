using FluentValidation;

namespace MLMS.Domain.Exams;

public class ExamValidator : AbstractValidator<Exam>
{
    public ExamValidator()
    {
        RuleFor(x => x.Questions)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.PassThresholdPoints)
            .GreaterThan(0);
        
        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0);

        RuleForEach(x => x.Questions)
            .NotNull()
            .SetValidator(new QuestionValidator());
    }
}