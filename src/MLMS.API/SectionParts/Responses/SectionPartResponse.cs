using MLMS.API.Exams.Requests;
using MLMS.API.Exams.Responses;
using MLMS.API.Files.Responses;
using MLMS.Domain.SectionParts;

namespace MLMS.API.SectionParts.Responses;

public class SectionPartResponse
{
    public long Id { get; set; }
    
    public string Title { get; set; }
    
    public int Index { get; set; }
    
    public long SectionId { get; set; }
    
    public MaterialType MaterialType { get; set; }
    
    public FileResponse? File { get; set; }
    
    public string? Link { get; set; }
    
    public ExamResponse? Exam { get; set; }
    
    public SectionPartStatus Status { get; set; }
}