using MLMS.Domain.Courses;
using MLMS.Domain.Users;

namespace MLMS.Domain.UsersCourses;

public class UserCourse
{
    public int UserId { get; set; }
    
    public User User { get; set; }
    
    public long CourseId { get; set; }
    
    public Course Course { get; set; }
    
    public UserCourseStatus Status { get; set; }
    
    public DateTime? FinishedAtUtc { get; set; }

    public DateTime? StartedAtUtc { get; set; }
}