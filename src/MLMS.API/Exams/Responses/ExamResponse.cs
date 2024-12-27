using MLMS.API.Exams.Requests;

namespace MLMS.API.Exams.Responses;

public class ExamResponse
{
    public long Id { get; set; }
    
    public int DurationMinutes { get; set; }
    
    public int PassThresholdPoints { get; set; }
    
    public List<QuestionResponse> Questions { get; set; } = [];
}