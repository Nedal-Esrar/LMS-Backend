using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Courses;
using MLMS.Domain.Departments;

namespace MLMS.Domain.Majors;

public class Major : EntityBase<int>
{
    public string Name { get; set; }
    
    public int DepartmentId { get; set; }
    
    public Department Department { get; set; }

    public List<CourseAssignment> CourseAssignments { get; set; } = [];
}