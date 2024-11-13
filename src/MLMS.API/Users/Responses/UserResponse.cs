using MLMS.API.Departments.Requests;
using MLMS.API.Files.Responses;
using MLMS.API.Majors.Requests;
using MLMS.Domain.Identity;

namespace MLMS.API.Users.Responses;

public class UserResponse
{
    public int Id { get; set; }
    
    public string WorkId { get; set; }
    
    public string FirstName { get; set; }
    
    public string MiddleName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public Gender Gender { get; set; }
    
    public DateOnly BirthDate { get; set; }
    
    public EducationalLevel EducationalLevel { get; set; }

    public UserRole Role { get; set; }
    
    public MajorResponse Major { get; set; }
    
    public DepartmentResponse Department { get; set; }
    
    public FileResponse? ProfilePicture { get; set; }
}