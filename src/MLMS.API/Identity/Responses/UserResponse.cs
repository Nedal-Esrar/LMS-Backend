using MLMS.API.Departments.Requests;
using MLMS.API.Majors.Requests;
using MLMS.Domain.Departments;
using MLMS.Domain.Enums;
using MLMS.Domain.Majors;

namespace MLMS.API.Identity.Responses;

public class UserResponse
{
    public int Id { get; set; }
    
    public string WorkId { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public Gender Gender { get; set; }
    
    public DateOnly BirthDate { get; set; }
    
    public EducationalLevel EducationalLevel { get; set; }

    public UserRole Role { get; set; }
    
    public MajorResponse Major { get; set; }
    
    public DepartmentResponse Department { get; set; }
}