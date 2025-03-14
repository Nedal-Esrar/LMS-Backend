using MLMS.API.Exams.Requests;
using MLMS.Domain.SectionParts;

namespace MLMS.API.SectionParts.Requests;

public class CreateSectionPartRequest
{
    public string Title { get; set; }
    
    public MaterialType MaterialType { get; set; }
    
    public Guid? FileId { get; set; }
    
    public string? Link { get; set; }
    
    public ExamCreateRequest? Exam { get; set; }
}