using MLMS.Domain.SectionParts;

namespace MLMS.API.SectionParts.Requests;

public class ChangeSectionPartStatusForUserRequest
{
    public SectionPartStatus Status { get; set; }
}