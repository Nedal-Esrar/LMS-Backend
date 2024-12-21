using MLMS.Domain.Common.Models;
using MLMS.Domain.Departments;
using MLMS.Domain.Majors;
using MLMS.Domain.Sections;
using MLMS.Domain.Users;
using MLMS.Domain.UsersCourses;

namespace MLMS.Domain.Courses;

public class Course : EntityBase<long>
{
    public string Name { get; set; }
    
    public double ExpectedTimeToFinishHours { get; set; }
    
    public int? ExpirationMonths { get; set; }

    public List<Department> AssignedDepartments { get; set; } = [];

    public List<Major> AssignedMajors { get; set; } = [];

    public List<Section> Sections { get; set; } = [];
    
    public List<UserCourse> UsersCourses { get; set; } = [];

    public DateTime CreatedAtUtc { get; set; }
    
    public DateTime? UpdatedAtUtc { get; set; }

    public int CreatedById { get; set; }
    
    public User CreatedBy { get; set; }
}