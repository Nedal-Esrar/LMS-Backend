using ErrorOr;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Departments;
using MLMS.Domain.Files;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Majors;
using MLMS.Domain.UsersCourses;
using Sieve.Models;

namespace MLMS.Domain.Users;

public class UserService(
    IUserRepository userRepository,
    IDepartmentRepository departmentRepository,
    IMajorRepository majorRepository,
    IFileRepository fileRepository,
    IUserContext userContext,
    IDbTransactionProvider dbTransactionProvider,
    IFileHandler fileHandler,
    ICourseAssignmentRepository courseAssignmentRepository,
    IUserCourseRepository userCourseRepository) : IUserService
{
    private readonly UserValidator _userValidator = new();
    
    public async Task<ErrorOr<None>> UpdateAsync(int id, User user)
    {
        var validationResult = await _userValidator.ValidateAsync(user);

        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }

        var userToUpdate = await userRepository.GetByIdAsync(id);

        if (userToUpdate is null)
        {
            return UserErrors.NotFound;
        }
        
        if (userToUpdate.WorkId != user.WorkId && await userRepository.ExistsByWorkIdAsync(user.WorkId))
        {
            return UserErrors.WorkIdExists;
        }
        
        if (!await departmentRepository.ExistsAsync(user.DepartmentId!.Value))
        {
            return DepartmentErrors.NotFound;
        }
        
        if (!await majorRepository.ExistsAsync(user.DepartmentId.Value, user.MajorId!.Value))
        {
            return MajorErrors.NotFound;
        }

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            if (user.MajorId != userToUpdate.MajorId)
            {
                var oldMajorAssignments = userToUpdate.MajorId.HasValue
                    ? (await courseAssignmentRepository.GetByMajorIdAsync(userToUpdate.MajorId!.Value))
                    .Select(ca => ca.CourseId)
                    .ToHashSet()
                    : new HashSet<long>();

                var newMajorAssignments = user.MajorId.HasValue
                    ? (await courseAssignmentRepository.GetByMajorIdAsync(user.MajorId!.Value))
                    .Select(ca => ca.CourseId)
                    .ToHashSet()
                    : new HashSet<long>();
                
                var coursesToAdd = newMajorAssignments.Except(oldMajorAssignments).ToList();
                var coursesToRemove = oldMajorAssignments.Except(newMajorAssignments).ToList();

                await userCourseRepository.CreateAsync(coursesToAdd.Select(c => new UserCourse
                {
                    CourseId = c,
                    UserId = id
                }).ToList());

                await userCourseRepository.DeleteAsync(coursesToRemove.Select(c => new UserCourse
                {
                    CourseId = c,
                    UserId = id
                }).ToList());
            }
            
            userToUpdate.MapUpdatedUser(user);

            await userRepository.UpdateAsync(userToUpdate);
        });

        return None.Value;
    }

    public async Task<ErrorOr<User>> GetByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        return user;
    }

    public async Task<ErrorOr<PaginatedList<User>>> GetAsync(SieveModel sieveModel)
    {
        return await userRepository.GetAsync(sieveModel);
    }

    public async Task<ErrorOr<None>> SetProfilePictureAsync(Guid imageId)
    {
        var image = await fileRepository.GetByIdAsync(imageId);

        if (image is null)
        {
            return FileErrors.NotFound;
        }

        if (!image.IsImage())
        {
            return FileErrors.NotImage;
        }
        
        var user = await userRepository.GetByIdAsync(userContext.Id);
        
        var oldImage = user!.ProfilePictureId is not null ? 
            await fileRepository.GetByIdAsync(user.ProfilePictureId!.Value)
            : null;

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            if (user!.ProfilePictureId.HasValue)
            {
                await fileRepository.DeleteAsync(user.ProfilePictureId!.Value);
            }

            user.ProfilePictureId = imageId;

            await userRepository.UpdateAsync(user);
            
            if (oldImage is not null)
            {
                await fileHandler.DeleteAsync(oldImage!.Path);
            }
        });

        return None.Value;
    }

    public async Task<ErrorOr<None>> ChangeUserStatusAsync(int id, bool isActive)
    {
        if (!await userRepository.ExistsAsync(id))
        {
            return UserErrors.NotFound;
        }
        
        await userRepository.ChangeUserStatusAsync(id, isActive);

        return None.Value;
    }
}