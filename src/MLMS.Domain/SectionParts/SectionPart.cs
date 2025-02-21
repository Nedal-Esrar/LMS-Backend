using MLMS.Domain.Common.Models;
using MLMS.Domain.Exams;
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
    
    public List<UserSectionPart> UserSectionPartStatuses { get; set; } = [];
    
    public MaterialType MaterialType { get; set; }
    
    public Guid? FileId { get; set; }
    
    public File? File { get; set; }
    
    public string? Link { get; set; }
    
    public long? ExamId { get; set; }
    
    public Exam? Exam { get; set; }
}