using MLMS.Domain.Common.Models;
using MLMS.Domain.SectionParts;
using File = MLMS.Domain.Files.File;

namespace MLMS.Domain.Exams;

public class Question : EntityBase<long>
{
    public string Text { get; set; }
    
    public Guid? ImageId { get; set; }
    
    public File? Image { get; set; }
    
    public int Points { get; set; }
    
    public Exam Exam { get; set; }
    
    public long ExamId { get; set; }

    public List<Choice> Choices { get; set; } = [];
    
    public int Index { get; set; }
}