using Riok.Mapperly.Abstractions;

namespace MLMS.Domain.Courses;

[Mapper]
public static partial class CourseMapper
{
    public static partial void MapForUpdate(this Course existingCourse, Course updatedCourse);
}