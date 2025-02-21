using MLMS.Domain.Courses;
using Sieve.Services;

namespace MLMS.Infrastructure.Courses;

public class CourseSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Course>(x => x.Name)
            .CanFilter()
            .CanSort();

        mapper.Property<Course>(x => x.CreatedAtUtc)
            .CanSort();
        
        mapper.Property<Course>(x => x.ExpectedTimeToFinishHours)
            .CanFilter()
            .CanSort();

        mapper.Property<Course>(x => x.ExpirationMonths)
            .CanFilter()
            .CanSort();
    }
}