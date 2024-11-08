using ErrorOr;
using MLMS.Domain.Common;
using MLMS.Domain.Departments;
using MLMS.Domain.Entities;
using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Majors;

namespace MLMS.Domain.Users;

public class UserService(
    IUserRepository userRepository,
    IDepartmentRepository departmentRepository,
    IMajorRepository majorRepository) : IUserService
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
        
        if (!await departmentRepository.ExistsAsync(user.DepartmentId))
        {
            return DepartmentErrors.NotFound;
        }
        
        if (!await majorRepository.ExistsAsync(user.DepartmentId, user.MajorId))
        {
            return MajorErrors.NotFound;
        }

        userToUpdate.MapUpdatedUser(user);

        await userRepository.UpdateAsync(userToUpdate);

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

    public async Task<ErrorOr<None>> DeleteAsync(int id)
    {
        if (!await userRepository.ExistsAsync(id))
        {
            return UserErrors.NotFound;
        }

        await userRepository.DeleteAsync(id);
        
        return None.Value;
    }
}