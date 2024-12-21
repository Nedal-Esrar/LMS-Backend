using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Questions;

public class Choice : EntityBase<long>
{
    public string Text { get; set; }
    
    public bool IsCorrect { get; set; }
}