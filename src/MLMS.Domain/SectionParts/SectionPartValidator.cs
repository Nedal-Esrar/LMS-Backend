using FluentValidation;
using MLMS.Domain.Exams;

namespace MLMS.Domain.SectionParts;

public class SectionPartValidator : AbstractValidator<SectionPart>
{
    public SectionPartValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        
        RuleFor(x => x.MaterialType).IsInEnum();
        
        RuleFor(x => x.Exam)
            .SetValidator(new ExamValidator())
            .When(x => x.MaterialType == MaterialType.Exam);
        
        RuleFor(x => x.FileId)
            .NotEmpty()
            .When(x => x.MaterialType == MaterialType.File);

        RuleFor(x => x.Link)
            .NotEmpty()
            .Must(BeAValidUrl)
            .When(x => x.MaterialType == MaterialType.Link);
    }
    
    private bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }
        
        return Uri.TryCreate(url, UriKind.Absolute, out var uri) 
               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}