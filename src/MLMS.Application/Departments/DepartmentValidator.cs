using FluentValidation;
using MLMS.Domain.Departments;

namespace MLMS.Application.Departments;

public class DepartmentValidator : AbstractValidator<Department>
{
    public DepartmentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}