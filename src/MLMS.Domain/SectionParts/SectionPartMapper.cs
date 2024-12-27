using Riok.Mapperly.Abstractions;

namespace MLMS.Domain.SectionParts;

[Mapper]
public static partial class SectionPartMapper
{
    public static void MapForUpdate(this SectionPart existingSectionPart, SectionPart updatedSectionPart)
    {
        existingSectionPart.Title = updatedSectionPart.Title;
        existingSectionPart.MaterialType = updatedSectionPart.MaterialType;
        existingSectionPart.FileId = updatedSectionPart.FileId;
        existingSectionPart.Link = updatedSectionPart.Link;
        existingSectionPart.Exam = existingSectionPart.Exam;
    }
}