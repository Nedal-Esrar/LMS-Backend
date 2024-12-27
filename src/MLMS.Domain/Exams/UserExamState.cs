using MLMS.Domain.Exams;
using MLMS.Domain.SectionParts;

namespace MLMS.Domain.UserSectionParts;

public class UserExamState
{
    public int UserId { get; set; }
    
    public long ExamId { get; set; }
    
    public Exam Exam { get; set; }
    
    public ExamStatus Status { get; set; }
}