using MLMS.Domain.Courses;

namespace MLMS.Domain.UsersCourses;

public class UserCourse
{
    public int UserId { get; set; }
    
    public long CourseId { get; set; }
    
    public Course Course { get; set; }
    
    public UserCourseStatus Status { get; set; }
    
    public DateTime? FinishedAtUtc { get; set; }
}