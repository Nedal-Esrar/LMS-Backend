using MLMS.Domain.Common.Models;
using MLMS.Domain.Questions;
using MLMS.Domain.Sections;
using MLMS.Domain.UserSectionParts;
using File = MLMS.Domain.Files.File;

namespace MLMS.Domain.SectionParts;

public class SectionPart : EntityBase<long>
{
    public string Title { get; set; }
    
    public int Index { get; set; }
    
    public long SectionId { get; set; }
    
    public Section Section { get; set; }
    
    public List<UserSectionPartDone> UserSectionPartStatuses { get; set; } = [];
    
    public List<UserSectionPartExamState> UserExamStates { get; set; } = [];
    
    public MaterialType MaterialType { get; set; }
    
    public Guid? FileId { get; set; }
    
    public File? File { get; set; }
    
    public string? Link { get; set; }
    
    public int? PassThresholdPoints { get; set; }
    
    public List<Question> Questions { get; set; } = [];
}