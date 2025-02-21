namespace MLMS.API.Exams.Responses;

public class ChoiceResponse
{
    public long Id { get; set; }
    
    public string Text { get; set; }
    
    public bool IsCorrect { get; set; }
}