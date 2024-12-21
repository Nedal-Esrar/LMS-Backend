using MLMS.API.Courses.Requests;
using MLMS.Domain.Courses;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.Courses;

[Mapper]
public static partial class CourseMapper
{
    public static partial Course ToDomain(this InitializeCourseRequest request);
}