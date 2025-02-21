using MLMS.Domain.Common.Models;
using MLMS.Domain.Courses;
using MLMS.Domain.SectionParts;

namespace MLMS.Domain.Sections;

public class Section : EntityBase<long>
{
    public string Title { get; set; }
    
    public int Index { get; set; }
    
    public long CourseId { get; set; }
    
    public Course Course { get; set; }

    public List<SectionPart> SectionParts { get; set; } = [];
}