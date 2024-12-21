using ErrorOr;

namespace MLMS.Domain.UsersCourses;

public interface IUserCourseRepository
{
    Task CreateAsync(List<UserCourse> userCourseEntities);
    
    Task DeleteByCourseAndUserIdsAsync(long id, List<int> toList);
    
    Task<bool> ExistsByCourseAndUserIdsAsync(long id, int userId);
    
    Task<UserCourse> GetByUserAndCourseAsync(long id, int userId);
}