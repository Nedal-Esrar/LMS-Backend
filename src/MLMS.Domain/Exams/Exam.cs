using MLMS.Domain.Common.Models;
using MLMS.Domain.SectionParts;

namespace MLMS.Domain.Exams;

public class Exam : EntityBase<long>
{
    public int DurationMinutes { get; set; }
    
    public int PassThresholdPoints { get; set; }
    
    public long SectionPartId { get; set; }
    
    public SectionPart SectionPart { get; set; }

    public List<Question> Questions { get; set; } = [];
}