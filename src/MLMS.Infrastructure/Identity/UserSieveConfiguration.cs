using MLMS.Infrastructure.Identity.Models;
using Sieve.Services;

namespace MLMS.Infrastructure.Identity;

public class UserSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<ApplicationUser>(u => u.WorkId)
            .CanFilter();

        mapper.Property<ApplicationUser>(u => u.FirstName)
            .CanFilter();
        
        mapper.Property<ApplicationUser>(u => u.MiddleName)
            .CanFilter();

        mapper.Property<ApplicationUser>(u => u.LastName)
            .CanFilter();
        
        mapper.Property<ApplicationUser>(u => u.Email)
            .CanFilter();
        
        mapper.Property<ApplicationUser>(u => u.PhoneNumber)
            .CanFilter();
        
        mapper.Property<ApplicationUser>(u => u.Gender)
            .CanFilter()
            .CanSort();
        
        mapper.Property<ApplicationUser>(u => u.EducationalLevel)
            .CanFilter()
            .CanSort();

        mapper.Property<ApplicationUser>(u => u.Role.Name)
            .HasName("role")
            .CanFilter()
            .CanSort();

        mapper.Property<ApplicationUser>(u => u.Major.Name)
            .HasName("major")
            .CanFilter();

        mapper.Property<ApplicationUser>(u => u.Department.Name)
            .HasName("department")
            .CanFilter();
    }
}