using MLMS.API.SectionParts.Models;
using MLMS.Domain.SectionParts;

namespace MLMS.API.SectionParts.Requests;

public class UpdateSectionPartRequest
{
    public string Title { get; set; }
    
    public MaterialType MaterialType { get; set; }
    
    public Guid? FileId { get; set; }
    
    public string? Link { get; set; }
    
    public List<QuestionRequestModel> Questions { get; set; } = [];
}