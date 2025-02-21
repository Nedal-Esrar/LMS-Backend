using MLMS.API.Departments.Requests;
using MLMS.Domain.Departments;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.Departments;

[Mapper]
public static partial class DepartmentMapper
{
    public static partial Department ToDomain(this CreateDepartmentRequest request);
    
    public static partial DepartmentResponse ToContract(this Department department);
    
    public static partial Department ToDomain(this UpdateDepartmentRequest request);
}