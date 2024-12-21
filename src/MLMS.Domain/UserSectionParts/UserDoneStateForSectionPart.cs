using MLMS.Domain.SectionParts;
using MLMS.Domain.Users;

namespace MLMS.Domain.UserSectionParts;

public class UserDoneStateForSectionPart
{
    public int UserId { get; set; }
    
    public User User { get; set; }
    
    public long SectionPartId { get; set; }
    
    public SectionPart SectionPart { get; set; }
    
    public bool IsDone { get; set; }
}