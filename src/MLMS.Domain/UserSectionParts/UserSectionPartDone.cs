using MLMS.Domain.SectionParts;
using MLMS.Domain.Users;

namespace MLMS.Domain.UserSectionParts;

public class UserSectionPartDone
{
    public int UserId { get; set; }
    
    public long SectionPartId { get; set; }
    
    public SectionPart SectionPart { get; set; }
    
    public bool IsDone { get; set; }
}