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
    
    Task<ErrorOr<(bool IsFinished, DateTime? FinishedAtUtc)>> CheckIfFinishedAsync(long id);
    
    Task<ErrorOr<None>> StartAsync(long id);
    
    Task<ErrorOr<None>> UpdateAsync(long id, Course course);
    
    Task<ErrorOr<None>> DeleteAsync(long id);
    
    Task<ErrorOr<List<CourseAssignment>>> GetAssignmentsByIdAsync(long id);
    
    Task<ErrorOr<Dictionary<UserCourseStatus, int>>> GetCourseStatusForCurrentUserAsync();
    
    Task<ErrorOr<PaginatedList<UserCourse>>> GetParticipantsByIdAsync(long id, SieveModel sieveModel);
    
    Task<ErrorOr<None>> NotifyParticipantAsync(long id, int userId);
    
    Task<ErrorOr<None>> ChangeManagerAsync(long id, int subAdminId);
    
    Task<ErrorOr<None>> CheckExistenceAsync(long id);
}