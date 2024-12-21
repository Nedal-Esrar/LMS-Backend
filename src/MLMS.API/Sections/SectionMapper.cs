using MLMS.API.Sections.Requests;
using MLMS.Domain.Sections;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.Sections;

[Mapper]
public static partial class SectionMapper
{
    public static Section ToDomain(this CreateSectionRequest request, long courseId)
    {
        var section = request.ToDomainInternal();

        section.CourseId = courseId;

        return section;
    }
    
    private static partial Section ToDomainInternal(this CreateSectionRequest request);

    public static Section ToDomain(this UpdateSectionRequest request, long courseId)
    {
        var section = request.ToDomainInternal();

        section.CourseId = courseId;

        return section;
    }
    
    private static partial Section ToDomainInternal(this UpdateSectionRequest request);
}