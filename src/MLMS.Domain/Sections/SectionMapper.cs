using Riok.Mapperly.Abstractions;

namespace MLMS.Domain.Sections;

[Mapper]
public static partial class SectionMapper
{
    public static void MapForUpdate(this Section existingSection, Section updateSection)
    {
        existingSection.Title = updateSection.Title;
    }
}