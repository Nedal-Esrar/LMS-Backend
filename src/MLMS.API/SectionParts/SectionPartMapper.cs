using MLMS.API.SectionParts.Models;
using MLMS.API.SectionParts.Requests;
using MLMS.API.SectionParts.Responses;
using MLMS.Domain.Exams;
using MLMS.Domain.SectionParts;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.SectionParts;

[Mapper]
public static partial class SectionPartMapper
{
    public static SectionPart ToDomain(this CreateSectionPartRequest request, long sectionId)
    {
        var sectionPart = request.ToDomainInternal();
        
        sectionPart.SectionId = sectionId;

        return sectionPart;
    }
    
    public static SectionPart ToDomain(this UpdateSectionPartRequest request, long sectionId)
    {
        var sectionPart = request.ToDomainInternal();
        
        sectionPart.SectionId = sectionId;

        return sectionPart;
    }
    
    public static partial SectionPartResponse ToContract(this SectionPart sectionPart);
    
    private static partial SectionPart ToDomainInternal(this UpdateSectionPartRequest request);
    
    private static partial SectionPart ToDomainInternal(this CreateSectionPartRequest request);

    private static partial Question ToDomain(this QuestionContractModel contract);
    
    private static partial Choice ToDomain(this ChoiceContractModel contract);
}