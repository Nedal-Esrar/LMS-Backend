namespace MLMS.Domain.Exams;

public class ExamSessionState
{
    public List<(long QuestionId, bool IsAnswered)> Questions { get; set; } = [];
    
    public long CheckpointQuestionId { get; set; }
}