namespace MLMS.API.Exams.Requests;

public class ChoiceCreateRequest
{
    public string Text { get; set; }
    
    public bool IsCorrect { get; set; }
}