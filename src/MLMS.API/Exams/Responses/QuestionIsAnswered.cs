namespace MLMS.API.Exams.Responses;

public class QuestionIsAnswered
{
    public long Id { get; set; }
    
    public int Index { get; set; }
    
    public bool IsAnswered { get; set; }
}