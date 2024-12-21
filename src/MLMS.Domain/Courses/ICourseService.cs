using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.UsersCourses;
using Sieve.Models;

namespace MLMS.Domain.Courses;

public interface ICourseService
{
    Task<ErrorOr<Course>> InitializeAsync(Course course);
    
    Task<ErrorOr<Course>> GetByIdAsync(long id);
    
    Task<ErrorOr<None>> EditAssignmentsAsync(long id, List<(int DepartmentId, int MajorId)> newAssignments);
    
    Task<ErrorOr<PaginatedList<Course>>> GetAsync(SieveModel sieveModel);
    
    Task<ErrorOr<UserCourseStatus>> FinishAsync(long id);
    
    Task<ErrorOr<bool>> CheckIfFinishedAsync(long id);
}