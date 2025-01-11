namespace MLMS.Domain.Exams;

public class ExamSessionState
{
    public List<(long QuestionId, bool IsAnswered, int Index)> Questions { get; set; } = [];
    
    public long CheckpointQuestionId { get; set; }
    
    public DateTime StartDateUtc { get; set; }
    
    public int DurationMinutes { get; set; }
}