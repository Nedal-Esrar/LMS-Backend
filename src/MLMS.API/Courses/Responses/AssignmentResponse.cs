using MLMS.API.Departments.Requests;

namespace MLMS.API.Courses.Responses;

public class AssignmentResponse
{
    public int MajorId { get; set; }
    
    public string MajorName { get; set; }
    
    public DepartmentResponse Department { get; set; }
}