using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.UsersCourses;
using Sieve.Models;

namespace MLMS.Domain.CourseAssignments;

public class CourseAssignmentService(
    ICourseAssignmentRepository courseAssignmentRepository,
    IUserCourseRepository userCourseRepository) : ICourseAssignmentService
{
    public async Task<ErrorOr<None>> UpdateByUserAsync(int userId, int oldMajorId, int newMajorId)
    {
        var oldMajorAssignments = (await courseAssignmentRepository.GetByMajorIdAsync(oldMajorId))
            .Select(ca => ca.CourseId)
            .ToHashSet();

        var newMajorAssignments = (await courseAssignmentRepository.GetByMajorIdAsync(newMajorId))
            .Select(ca => ca.CourseId)
            .ToHashSet();
                
        var coursesToAdd = newMajorAssignments.Except(oldMajorAssignments).ToList();
        var coursesToRemove = oldMajorAssignments.Except(newMajorAssignments).ToList();

        await userCourseRepository.CreateAsync(coursesToAdd.Select(c => new UserCourse
        {
            CourseId = c,
            UserId = userId
        }).ToList());

        await userCourseRepository.DeleteAsync(coursesToRemove.Select(c => new UserCourse
        {
            CourseId = c,
            UserId = userId
        }).ToList());

        return None.Value;
    }

    public async Task<ErrorOr<None>> CreateAssignmentsAsync(int userId, int majorId)
    {
        var courseAssignments = 
            await courseAssignmentRepository.GetByMajorIdAsync(majorId);

        var userCourseEntities = courseAssignments.Select(courseAssignment => 
            new UserCourse
            {
                UserId = userId, 
                CourseId = courseAssignment.CourseId, 
                Status = UserCourseStatus.NotStarted
            }).ToList();

        await userCourseRepository.CreateAsync(userCourseEntities);

        return None.Value;
    }

    public async Task<ErrorOr<bool>> IsAssignedToUserAsync(long courseId, int userId)
    {
        return await userCourseRepository.ExistsByCourseAndUserIdsAsync(courseId, userId);
    }

    public async Task<ErrorOr<UserCourse>> GetUserCourseAsync(long courseId, int userId)
    {
        return await userCourseRepository.GetByUserAndCourseAsync(courseId, userId);
    }

    public async Task<ErrorOr<None>> UpdateUserCoursesAsync(List<UserCourse> userCourses)
    {
        await userCourseRepository.UpdateAsync(userCourses);

        return None.Value;
    }

    public async Task<ErrorOr<PaginatedList<UserCourse>>> GetUserCourseByCourseIdAsync(long courseId, bool isSubAdminRequesting, SieveModel sieveModel)
    {
        return await userCourseRepository.GetByCourseIdAsync(courseId, isSubAdminRequesting, sieveModel);
    }

    public async Task<ErrorOr<List<UserCourse>>> GetUserCoursesByUserIdAsync(int userId)
    {
        return await userCourseRepository.GetByUserIdAsync(userId);
    }

    public async Task<ErrorOr<List<CourseAssignment>>> GetCourseAssignmentsByCourseIdAsync(long id, bool includeMajorAssignments)
    {
        return await courseAssignmentRepository.GetByCourseIdAsync(id, includeMajorAssignments);
    }

    public async Task<ErrorOr<None>> CreateCourseAssignmentsAsync(List<CourseAssignment> courseAssignments)
    {
        await courseAssignmentRepository.CreateAsync(courseAssignments);
        
        return None.Value;
    }

    public async Task<ErrorOr<None>> CreateUserCoursesAsync(List<UserCourse> userCourses)
    {
        await userCourseRepository.CreateAsync(userCourses);
        
        return None.Value;
    }

    public async Task<ErrorOr<None>> DeleteCourseAssignmentsAsync(List<CourseAssignment> courseAssignments)
    {
        await courseAssignmentRepository.DeleteAsync(courseAssignments);
        
        return None.Value;
    }

    public async Task<ErrorOr<None>> DeleteUserCoursesAsync(long courseId, List<int> usersToUnassign)
    {
        await userCourseRepository.DeleteByCourseAndUserIdsAsync(courseId, usersToUnassign);
        
        return None.Value;
    }

    public async Task<ErrorOr<List<UserCourse>>> GetStatesForFinishedCoursesWithExpirationAsync()
    {
        return await userCourseRepository.GetStatesForFinishedCoursesWithExpirationAsync();
    }
}