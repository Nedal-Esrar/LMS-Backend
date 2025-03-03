using ErrorOr;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Exceptions;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Courses;
using MLMS.Domain.Departments;
using MLMS.Domain.Files;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Majors;
using MLMS.Domain.UsersCourses;
using Sieve.Models;

namespace MLMS.Domain.Users;

public class UserService(
    IUserRepository userRepository,
    IDepartmentService departmentService,
    IMajorService majorService,
    IFileService fileService,
    IUserContext userContext,
    IDbTransactionProvider dbTransactionProvider,
    ICourseAssignmentService courseAssignmentService) : IUserService
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

        var departmentExistenceResult = await departmentService.CheckExistenceAsync(user.DepartmentId!.Value);
        
        if (departmentExistenceResult.IsError)
        {
            return departmentExistenceResult.Errors;
        }
        
        var majorExistenceResult = await majorService.CheckExistenceAsync(user.DepartmentId!.Value, user.MajorId!.Value);
        
        if (majorExistenceResult.IsError)
        {
            return majorExistenceResult.Errors;
        }

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            if (user.MajorId != userToUpdate.MajorId)
            {
                var assignmentResult = await courseAssignmentService.UpdateByUserAsync(id, userToUpdate.MajorId!.Value, user.MajorId!.Value);

                if (assignmentResult.IsError)
                {
                    throw new UnoccasionalErrorException(assignmentResult.Errors);
                }
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
        var imageRetrievalResult = await fileService.GetByIdAsync(imageId);

        if (imageRetrievalResult.IsError)
        {
            return imageRetrievalResult.Errors;
        }

        if (!imageRetrievalResult.Value.IsImage())
        {
            return FileErrors.NotImage;
        }
        
        var user = await userRepository.GetByIdAsync(userContext.Id);

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            var oldImageId = user!.ProfilePictureId;
            
            user!.ProfilePictureId = imageId;

            await userRepository.UpdateAsync(user);
            
            if (oldImageId.HasValue)
            {
                await fileService.DeleteAsync(oldImageId!.Value);
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

    public async Task<ErrorOr<List<User>>> GetBySectionIdAsync(long sectionId)
    {
        return await userRepository.GetBySectionIdAsync(sectionId);
    }

    public async Task<ErrorOr<bool>> CheckExistenceByWorkIdAsync(string workId)
    {
        return await userRepository.ExistsByWorkIdAsync(workId);
    }

    public async Task<ErrorOr<int>> CreateAsync(User user, string password)
    {
        return await userRepository.CreateAsync(user, password);
    }

    public async Task<ErrorOr<User>> GetByWorkIdAsync(string workId)
    {
        var user = await userRepository.GetByWorkIdAsync(workId);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        return user;
    }

    public async Task<ErrorOr<None>> CheckExistenceByIdAsync(int id)
    {
        var userExists = await userRepository.ExistsAsync(id);
        
        return userExists ? None.Value : UserErrors.NotFound;
    }

    public async Task<ErrorOr<None>> CheckIfSubAdminAsync(int subAdminId)
    {
        var isSubAdmin = await userRepository.IsSubAdminAsync(subAdminId);
        
        return isSubAdmin ? None.Value : UserErrors.NotSubAdmin;
    }

    public async Task<ErrorOr<List<User>>> GetByMajorsAsync(List<int> majorIds)
    {
        return await userRepository.GetByMajorsAsync(majorIds);
    }
}