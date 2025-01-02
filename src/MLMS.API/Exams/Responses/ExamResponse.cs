using MLMS.API.Exams.Requests;
using MLMS.Domain.Exams;
using MLMS.Domain.UserSectionParts;

namespace MLMS.API.Exams.Responses;

public class ExamResponse
{
    public long Id { get; set; }
    
    public int DurationMinutes { get; set; }
    
    public int PassThresholdPoints { get; set; }
    
    public int MaxGradePoints { get; set; }
    
    public int LastGottenGradePoints { get; set; }
    
    public ExamStatus Status { get; set; }
    
    public List<QuestionResponse> Questions { get; set; } = [];
}