using Riok.Mapperly.Abstractions;

namespace MLMS.Domain.Courses;

[Mapper]
public static partial class CourseMapper
{
    public static void MapForUpdate(this Course existingCourse, Course updatedCourse)
    {
        existingCourse.Name = updatedCourse.Name;
        existingCourse.ExpectedTimeToFinishHours = updatedCourse.ExpectedTimeToFinishHours;
        existingCourse.ExpirationMonths = updatedCourse.ExpirationMonths;
    }
}