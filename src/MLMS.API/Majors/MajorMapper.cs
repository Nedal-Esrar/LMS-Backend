using MLMS.API.Majors.Requests;
using MLMS.Domain.Majors;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.Majors;

[Mapper]
public static partial class MajorMapper
{
    public static Major ToDomain(this CreateMajorRequest request, int departmentId)
    {
        var major = request.ToDomainInternal();

        major.DepartmentId = departmentId;

        return major;
    }

    private static partial Major ToDomainInternal(this CreateMajorRequest request);
    
    public static partial MajorResponse ToContract(this Major major);

    public static partial Major ToDomain(this UpdateMajorRequest request);
}