using MLMS.Domain.Common.Models;
using Sieve.Models;

namespace MLMS.Domain.UsersCourses;

public interface IUserCourseRepository
{
    Task CreateAsync(List<UserCourse> userCourseEntities);
    
    Task DeleteByCourseAndUserIdsAsync(long id, List<int> toList);
    
    Task<bool> ExistsByCourseAndUserIdsAsync(long id, int userId);
    
    Task<UserCourse> GetByUserAndCourseAsync(long id, int userId);
    
    Task UpdateAsync(UserCourse userCourse);
    
    Task<List<UserCourse>> GetByUserIdAsync(int userId);
    
    Task<List<UserCourse>> GetStatesForFinishedCoursesWithExpirationAsync();
    
    Task UpdateAsync(List<UserCourse> userCourses);
    
    Task<PaginatedList<UserCourse>> GetByCourseIdAsync(long courseId, bool getOnlyCreatedByCurrentUser, SieveModel sieveModel);
}