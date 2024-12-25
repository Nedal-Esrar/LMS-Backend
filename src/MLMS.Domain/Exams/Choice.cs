using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Exams;

public class Choice : EntityBase<long>
{
    public string Text { get; set; }
    
    public bool IsCorrect { get; set; }
    
    public Question Question { get; set; }
    
    public long QuestionId { get; set; }
}