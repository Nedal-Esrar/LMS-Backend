using MLMS.Domain.UsersCourses;

namespace MLMS.API.Courses.Responses;

public class CourseDetailedResponse
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public double ExpectedTimeToFinishHours { get; set; }
    
    public int? ExpirationMonths { get; set; }
    
    public List<SectionDetailedResponse> Sections { get; set; } = [];
    
    public UserCourseStatus? Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    
    public DateTime? UpdatedAtUtc { get; set; }

    public int? CreatedById { get; set; }
    
    public string? CreatedByName { get; set; }
    
    public double Progress { get; set; }
    
    public DateTime? FinishedAtUtc { get; set; }
}