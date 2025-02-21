namespace MLMS.API.Exams.Responses;

public class ExamSessionStateResponse
{
    public List<QuestionIsAnswered> Questions { get; set; } = [];
    
    public long CheckpointQuestionId { get; set; }
    
    public DateTime StartDateUtc { get; set; }
    
    public int DurationMinutes { get; set; }
}