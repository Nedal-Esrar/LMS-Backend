using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Questions;

public class Question : EntityBase<long>
{
    public string Text { get; set; }
    
    public int Points { get; set; }

    public List<Choice> Choices { get; set; } = [];
}