using MLMS.Domain.Departments;
using Sieve.Services;

namespace MLMS.Infrastructure.Departments;

public class DepartmentSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Department>(d => d.Name)
            .CanFilter();
    }
}