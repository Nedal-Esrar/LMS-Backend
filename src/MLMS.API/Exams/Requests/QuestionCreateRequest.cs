namespace MLMS.API.Exams.Requests;

public class QuestionCreateRequest
{
    public string Text { get; set; }
    
    public int Points { get; set; }
    
    public Guid? ImageId { get; set; }

    public List<ChoiceCreateRequest> Choices { get; set; } = [];
}