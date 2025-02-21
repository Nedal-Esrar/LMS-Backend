using MLMS.Domain.Common.Models;
using MLMS.Domain.ExamSessions;
using MLMS.Domain.SectionParts;
using MLMS.Domain.UserSectionParts;

namespace MLMS.Domain.Exams;

public class Exam : EntityBase<long>
{
    public int DurationMinutes { get; set; }
    
    public int PassThresholdPoints { get; set; }

    public List<Question> Questions { get; set; } = [];
    
    public List<UserExamState> UserExamStates { get; set; } = [];
    
    public List<ExamSession> ExamSessions { get; set; } = [];
    
    public int MaxGradePoints { get; set; }
}