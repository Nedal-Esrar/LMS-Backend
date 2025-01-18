using MLMS.Domain.Common.Models;
using MLMS.Domain.Departments;
using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Enums;
using MLMS.Domain.Majors;
using File = MLMS.Domain.Files.File;

namespace MLMS.Domain.Users;

public class User : EntityBase<int>
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
    
    public int? MajorId { get; set; }
    
    public Major Major { get; set; }
    
    public int? DepartmentId { get; set; }
    
    public Department Department { get; set; }
    
    public Guid? ProfilePictureId { get; set; }
    
    public File ProfilePicture { get; set; }
}