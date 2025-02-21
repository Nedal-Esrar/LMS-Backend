using MLMS.API.Exams;
using MLMS.API.SectionParts.Requests;
using MLMS.API.SectionParts.Responses;
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
        sectionPart.Exam = request.Exam?.ToDomain();

        return sectionPart;
    }
    
    public static SectionPart ToDomain(this UpdateSectionPartRequest request, long sectionId)
    {
        var sectionPart = request.ToDomainInternal();
        
        sectionPart.SectionId = sectionId;
        sectionPart.Exam = request.Exam?.ToDomain();

        return sectionPart;
    }

    public static SectionPartResponse ToContract(this SectionPart sectionPart)
    {
        var response = sectionPart.ToContractInternal();
        
        response.Exam = sectionPart.Exam?.ToContract();
        response.Status = sectionPart.UserSectionPartStatuses.FirstOrDefault()?.Status ?? SectionPartStatus.NotViewed;

        return response;
    }
    
    private static partial SectionPart ToDomainInternal(this UpdateSectionPartRequest request);
    
    private static partial SectionPart ToDomainInternal(this CreateSectionPartRequest request);
    
    private static partial SectionPartResponse ToContractInternal(this SectionPart sectionPart);
}