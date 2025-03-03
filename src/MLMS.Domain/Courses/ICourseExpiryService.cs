using ErrorOr;
using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Courses;

public interface ICourseExpiryService
{
    Task<ErrorOr<None>> CheckCoursesExpiryAsync();
}