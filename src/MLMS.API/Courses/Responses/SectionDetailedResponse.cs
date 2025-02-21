using MLMS.API.SectionParts.Responses;

namespace MLMS.API.Courses.Responses;

public class SectionDetailedResponse
{
    public long Id { get; set; }
    
    public string Title { get; set; }
    
    public int Index { get; set; }

    public List<SectionPartResponse> SectionParts { get; set; } = [];
}