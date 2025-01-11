using FluentValidation;

namespace MLMS.Domain.Courses;

public class CourseValidator : AbstractValidator<Course>
{
    public CourseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.ExpectedTimeToFinishHours)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.ExpirationMonths)
            .GreaterThan(0);
    }
}