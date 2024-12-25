using MLMS.Domain.Common.Models;
using MLMS.Domain.SectionParts;

namespace MLMS.Domain.Exams;

public class Question : EntityBase<long>
{
    public string Text { get; set; }
    
    public int Points { get; set; }
    
    public SectionPart SectionPart { get; set; }
    
    public long SectionPartId { get; set; }

    public List<Choice> Choices { get; set; } = [];
}