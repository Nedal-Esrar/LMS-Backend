using Microsoft.Extensions.Options;
using MLMS.Domain.Majors;
using MLMS.Infrastructure.Departments;
using MLMS.Infrastructure.Identity;
using MLMS.Infrastructure.Majors;
using Sieve.Models;
using Sieve.Services;

namespace MLMS.Infrastructure.Common;

public class LmsSieveProcessor(IOptions<SieveOptions> options) : SieveProcessor(options)
{
    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        return mapper.ApplyConfiguration<UserSieveConfiguration>()
            .ApplyConfiguration<MajorSieveConfiguration>()
            .ApplyConfiguration<DepartmentSieveConfiguration>();
    }
}