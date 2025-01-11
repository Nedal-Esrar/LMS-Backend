using MLMS.Domain.UsersCourses;
using Sieve.Services;

namespace MLMS.Infrastructure.UserCourses;

public class UserCourseSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<UserCourse>(uc => uc.Status)
            .CanFilter();
    }
}