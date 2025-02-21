using MLMS.Domain.ExamSessions;

namespace MLMS.Domain.Exams;

public class ExamSessionQuestionChoice
{
    public Guid ExamSessionId { get; set; }
    
    public ExamSession ExamSession { get; set; }
    
    public long QuestionId { get; set; }
    
    public Question Question { get; set; }
    
    public long? ChoiceId { get; set; }
    
    public Choice? Choice { get; set; }
}