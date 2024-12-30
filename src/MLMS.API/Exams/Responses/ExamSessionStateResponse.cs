namespace MLMS.API.Exams.Responses;

public class ExamSessionStateResponse
{
    public List<(long QuestionId, bool IsAnswered)> Questions { get; set; } = [];
    
    public long CheckpointQuestionId { get; set; }
}