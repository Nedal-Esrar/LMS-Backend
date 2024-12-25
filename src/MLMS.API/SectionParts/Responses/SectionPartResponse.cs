using MLMS.API.Files.Responses;
using MLMS.API.SectionParts.Models;
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
    
    public int? PassThresholdPoints { get; set; }
    
    public List<QuestionContractModel> Questions { get; set; } = [];
}