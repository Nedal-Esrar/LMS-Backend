using Microsoft.AspNetCore.Identity;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Departments;
using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Enums;
using MLMS.Domain.Identity.Models;
using MLMS.Domain.Majors;
using File = MLMS.Domain.Files.File;

namespace MLMS.Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser<int>
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
    
    public int? MajorId { get; set; }
    
    public Major Major { get; set; }
    
    public int? DepartmentId { get; set; }
    
    public Department Department { get; set; }
    
    public List<RefreshToken> RefreshTokens { get; set; } = [];
    
    public int RoleId { get; set; }

    public IdentityRole<int> Role { get; set; }
    
    public Guid? ProfilePictureId { get; set; }
    
    public File? ProfilePicture { get; set; }
    
    public bool IsActive { get; set; }
}