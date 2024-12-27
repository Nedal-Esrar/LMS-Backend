namespace MLMS.API.Exams.Requests;

public class ExamUpdateRequest
{
    public long Id { get; set; }
    
    public int DurationMinutes { get; set; }
    
    public int PassThresholdPoints { get; set; }
    
    public List<QuestionUpdateRequest> Questions { get; set; } = [];
}