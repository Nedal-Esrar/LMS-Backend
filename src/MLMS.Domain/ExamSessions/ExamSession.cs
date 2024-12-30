using MLMS.Domain.Common.Models;
using MLMS.Domain.Exams;

namespace MLMS.Domain.ExamSessions;

public class ExamSession : EntityBase<Guid>
{
    public long ExamId { get; set; }
    
    public Exam Exam { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime StartDateUtc { get; set; }
    
    public long CheckpointQuestionId { get; set; }
    
    public bool IsDone { get; set; }
}