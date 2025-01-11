using Microsoft.Extensions.Options;
using MLMS.Domain.Majors;
using MLMS.Infrastructure.Courses;
using MLMS.Infrastructure.Departments;
using MLMS.Infrastructure.Identity;
using MLMS.Infrastructure.Majors;
using MLMS.Infrastructure.Notifications;
using MLMS.Infrastructure.UserCourses;
using Sieve.Models;
using Sieve.Services;

namespace MLMS.Infrastructure.Common;

public class LmsSieveProcessor(IOptions<SieveOptions> options) : SieveProcessor(options)
{
    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        return mapper.ApplyConfiguration<UserSieveConfiguration>()
            .ApplyConfiguration<MajorSieveConfiguration>()
            .ApplyConfiguration<DepartmentSieveConfiguration>()
            .ApplyConfiguration<NotificationSieveConfiguration>()
            .ApplyConfiguration<CourseSieveConfiguration>()
            .ApplyConfiguration<UserCourseSieveConfiguration>();
    }
}