using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Courses;

namespace MLMS.Domain.Departments;

public class Department : EntityBase<int>
{
    public string Name { get; set; }
}