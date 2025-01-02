using MLMS.Domain.UsersCourses;

namespace MLMS.API.Courses.Responses;

public class CourseParticipantResponse
{
    public int UserId { get; set; }
    
    public string UserName { get; set; }
    
    public UserCourseStatus Status { get; set; }
    
    public DateTime? StartedAtUtc { get; set; }
    
    public DateTime? FinishedAtUtc { get; set; }
}