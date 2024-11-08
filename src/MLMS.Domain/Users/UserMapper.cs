using MLMS.Domain.Identity;

namespace MLMS.Domain.Users;

public static class UserMapper
{
    public static void MapUpdatedUser(this User existingUser, User userUpdated)
    {
        existingUser.DepartmentId = userUpdated.DepartmentId;
        existingUser.FirstName = userUpdated.FirstName;
        existingUser.Email = userUpdated.Email;
        existingUser.MiddleName = userUpdated.MiddleName;
        existingUser.LastName = userUpdated.LastName;
        existingUser.Gender = existingUser.Gender;
        existingUser.BirthDate = existingUser.BirthDate;
        existingUser.EducationalLevel = userUpdated.EducationalLevel;
        existingUser.MajorId = userUpdated.MajorId;
        existingUser.PhoneNumber = userUpdated.PhoneNumber;
        existingUser.WorkId = userUpdated.WorkId;
    }
}