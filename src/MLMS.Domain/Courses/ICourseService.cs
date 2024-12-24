using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.UsersCourses;
using Sieve.Models;

namespace MLMS.Domain.Courses;

public interface ICourseService
{
    Task<ErrorOr<Course>> InitializeAsync(Course course);
    
    Task<ErrorOr<Course>> GetByIdAsync(long id);
    
    Task<ErrorOr<None>> EditAssignmentsAsync(long id, List<int> newAssignments);
    
    Task<ErrorOr<PaginatedList<Course>>> GetAsync(SieveModel sieveModel);
    
    Task<ErrorOr<UserCourseStatus>> FinishAsync(long id);
    
    Task<ErrorOr<bool>> CheckIfFinishedAsync(long id);
    
    Task<ErrorOr<None>> StartAsync(long id);
    
    Task<ErrorOr<None>> UpdateAsync(long id, Course course);
    
    Task<ErrorOr<None>> DeleteAsync(long id);
    
    Task<ErrorOr<List<CourseAssignment>>> GetAssignmentsByIdAsync(long id);
}