using Riok.Mapperly.Abstractions;

namespace MLMS.Domain.SectionParts;

[Mapper]
public static partial class SectionPartMapper
{
    [MapperIgnoreSource(nameof(SectionPart.SectionId))]
    [MapperIgnoreSource(nameof(SectionPart.Index))]
    public static partial void MapForUpdate(this SectionPart existingSectionPart, SectionPart updatedSectionPart);
}