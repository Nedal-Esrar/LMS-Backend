using MLMS.Domain.Courses;
using MLMS.Domain.Departments;
using MLMS.Domain.Majors;

namespace MLMS.Domain.CourseAssignments;

public class CourseAssignment
{
    public long CourseId { get; set; }
    
    public Course Course { get; set; }
    
    public int MajorId { get; set; }
    
    public Major Major { get; set; }
}