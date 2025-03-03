using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.UsersCourses;
using Sieve.Models;

namespace MLMS.Domain.CourseAssignments;

public interface ICourseAssignmentService
{
    Task<ErrorOr<None>> UpdateByUserAsync(int userId, int oldMajorId, int newMajorId);
    
    Task<ErrorOr<None>> CreateAssignmentsAsync(int userId, int majorId);
    
    Task<ErrorOr<bool>> IsAssignedToUserAsync(long courseId, int userId);
    
    Task<ErrorOr<UserCourse>> GetUserCourseAsync(long courseId, int userId);
    
    Task<ErrorOr<None>> UpdateUserCoursesAsync(List<UserCourse> userCourses);
    
    Task<ErrorOr<PaginatedList<UserCourse>>> GetUserCourseByCourseIdAsync(long courseId, bool isSubAdminRequesting, SieveModel sieveModel);
   
    Task<ErrorOr<List<UserCourse>>> GetUserCoursesByUserIdAsync(int userId);
    
    Task<ErrorOr<List<CourseAssignment>>> GetCourseAssignmentsByCourseIdAsync(long id, bool includeMajorAssignments = false);
    
    Task<ErrorOr<None>> CreateCourseAssignmentsAsync(List<CourseAssignment> courseAssignments);
    
    Task<ErrorOr<None>> CreateUserCoursesAsync(List<UserCourse> userCourses);
    
    Task<ErrorOr<None>> DeleteCourseAssignmentsAsync(List<CourseAssignment> courseAssignments);
    
    Task<ErrorOr<None>> DeleteUserCoursesAsync(long courseId, List<int> usersToUnassign);
    
    Task<ErrorOr<List<UserCourse>>> GetStatesForFinishedCoursesWithExpirationAsync();
}