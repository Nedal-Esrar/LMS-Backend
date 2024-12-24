using MLMS.Domain.Users;
using MLMS.Domain.UsersCourses;

namespace MLMS.API.Courses.Responses;

public class CourseSimplifiedResponse
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public double ExpectedTimeToFinishHours { get; set; }
    
    public int? ExpirationMonths { get; set; }
    
    public UserCourseStatus? Status { get; set; } // mapped for staff.

    public DateTime CreatedAtUtc { get; set; }
    
    public DateTime? UpdatedAtUtc { get; set; }
    
    public int CreatedById { get; set; } // mapped for admin.
    
    public string? CreatedByName { get; set; } // mapped for admin.
    
    public double Progress { get; set; } // mapped for staff.
}