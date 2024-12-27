namespace MLMS.API.Exams.Requests;

public class ExamCreateRequest
{
    public int DurationMinutes { get; set; }
    
    public int PassThresholdPoints { get; set; }
    
    public List<QuestionCreateRequest> Questions { get; set; } = [];
}