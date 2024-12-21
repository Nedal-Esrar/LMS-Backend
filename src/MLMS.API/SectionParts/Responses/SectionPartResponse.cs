using MLMS.API.Files.Responses;
using MLMS.API.SectionParts.Models;
using MLMS.Domain.Questions;
using MLMS.Domain.SectionParts;

namespace MLMS.API.SectionParts.Responses;

public class SectionPartResponse
{
    public string Title { get; set; }
    
    public int Index { get; set; }
    
    public long SectionId { get; set; }
    
    public MaterialType MaterialType { get; set; }
    
    public FileResponse? File { get; set; }
    
    public string? Link { get; set; }
    
    public int? PassThresholdPoints { get; set; }
    
    public List<QuestionRequestModel> Questions { get; set; } = [];
}