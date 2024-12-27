namespace MLMS.API.Exams.Requests;

public class ChoiceUpdateRequest
{
    public long Id { get; set; }
    
    public string Text { get; set; }
    
    public bool IsCorrect { get; set; }
}