namespace MLMS.API.Exams.Requests;

public class QuestionUpdateRequest
{
    public long Id { get; set; }
    
    public string Text { get; set; }
    
    public int Points { get; set; }
    
    public Guid? FileId { get; set; }

    public List<ChoiceUpdateRequest> Choices { get; set; } = [];
}