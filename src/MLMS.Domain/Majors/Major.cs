using MLMS.Domain.Departments;
using MLMS.Domain.Entities;

namespace MLMS.Domain.Majors;

public class Major : EntityBase<int>
{
    public string Name { get; set; }
    
    public int DepartmentId { get; set; }
    
    public Department Department { get; set; }
}