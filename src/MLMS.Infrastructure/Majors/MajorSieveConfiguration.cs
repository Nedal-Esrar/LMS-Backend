using MLMS.Domain.Majors;
using Sieve.Services;

namespace MLMS.Infrastructure.Majors;

public class MajorSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Major>(d => d.Name)
            .CanFilter();
    }
}