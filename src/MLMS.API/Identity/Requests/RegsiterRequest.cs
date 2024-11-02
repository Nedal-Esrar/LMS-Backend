using MLMS.Domain.Enums;

namespace MLMS.API.Identity.Requests;

public class RegisterRequest
{
    public string WorkId { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public Gender Gender { get; set; }
    
    public DateOnly BirthDate { get; set; }
    
    public EducationalLevel EducationalLevel { get; set; }

    public UserRole Role { get; set; }
    
    public int MajorId { get; set; }
    
    public int DepartmentId { get; set; }
}