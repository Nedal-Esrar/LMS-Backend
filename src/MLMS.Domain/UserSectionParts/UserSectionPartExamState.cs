using MLMS.Domain.SectionParts;
using MLMS.Domain.Users;

namespace MLMS.Domain.UserSectionParts;

public class UserSectionPartExamState
{
    public int UserId { get; set; }
    
    public long SectionPartId { get; set; }
    
    public SectionPart SectionPart { get; set; }
    
    public ExamStatus Status { get; set; }
}