using Microsoft.AspNetCore.Identity;
using MLMS.Domain.Departments;
using MLMS.Domain.Entities;
using MLMS.Domain.Enums;
using MLMS.Domain.Majors;

namespace MLMS.Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser<int>
{
    public string WorkId { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public Gender Gender { get; set; }
    
    public DateOnly BirthDate { get; set; }
    
    public EducationalLevel EducationalLevel { get; set; }

    public List<IdentityRole> Roles { get; set; } = [];
    
    public int MajorId { get; set; }
    
    public Major Major { get; set; }
    
    public int DepartmentId { get; set; }
    
    public Department Department { get; set; }
    
    public List<RefreshToken> RefreshTokens { get; set; } = [];
}