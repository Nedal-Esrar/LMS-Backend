using Microsoft.AspNetCore.Identity;
using MLMS.Domain.Identity;
using MLMS.Domain.Users;
using MLMS.Infrastructure.Identity.Models;
using Riok.Mapperly.Abstractions;

namespace MLMS.Infrastructure.Identity;

[Mapper]
public static partial class IdentityMapper
{
    public static User ToDomain(this ApplicationUser userEntity)
    {
        var user = userEntity.ToDomainInternal();

        user.Role = Enum.Parse<UserRole>(userEntity.Role.Name!);

        return user;
    }

    public static void MapForUpdate(this ApplicationUser userToUpdate, ApplicationUser updatedUser)
    {
        userToUpdate.WorkId = updatedUser.WorkId;
        userToUpdate.FirstName = updatedUser.FirstName;
        userToUpdate.MiddleName = updatedUser.MiddleName;
        userToUpdate.LastName = updatedUser.LastName;
        userToUpdate.Gender = updatedUser.Gender;
        userToUpdate.BirthDate = updatedUser.BirthDate;
        userToUpdate.EducationalLevel = updatedUser.EducationalLevel;
        userToUpdate.Email = updatedUser.Email;
        userToUpdate.PhoneNumber = updatedUser.PhoneNumber;
        userToUpdate.MajorId = updatedUser.MajorId;
        userToUpdate.DepartmentId = updatedUser.DepartmentId;
        userToUpdate.ProfilePictureId = updatedUser.ProfilePictureId;
    }
    
    [MapperIgnoreSource(nameof(userEntity.Role))]
    private static partial User ToDomainInternal(this ApplicationUser userEntity);

    [MapperIgnoreSource(nameof(user.Role))]
    public static partial ApplicationUser ToIdentityUser(this User user);
}