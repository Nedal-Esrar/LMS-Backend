using MLMS.Domain.UsersCourses;

namespace MLMS.Infrastructure.UserCourses;

public class UserCourseRepository : IUserCourseRepository
{
    public Task CreateAsync(List<UserCourse> userCourseEntities)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByCourseAndUserIdsAsync(long id, List<int> toList)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsByCourseAndUserIdsAsync(long id, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserCourse> GetByUserAndCourseAsync(long id, int userId)
    {
        throw new NotImplementedException();
    }
}