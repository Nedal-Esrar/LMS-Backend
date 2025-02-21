using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Enums;

namespace MLMS.API.Identity.Requests;

public class RegisterRequest
{
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
    
    public int MajorId { get; set; }
    
    public int DepartmentId { get; set; }
}