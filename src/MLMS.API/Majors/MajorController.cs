using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Departments.Requests;
using MLMS.API.Majors.Requests;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Majors;

using static MLMS.API.Common.AuthorizationPolicies;

namespace MLMS.API.Majors;

[Authorize]
[Route("departments/{departmentId:int}/majors")]
[ApiVersion("1.0")]
public class MajorController(IMajorService majorService) : ApiControllerBase
{
    [HttpPost]
    [Authorize(Policy = SuperAdmin)]
    public async Task<IActionResult> Create(int departmentId, CreateMajorRequest request)
    {
        var result = await majorService.CreateAsync(request.ToDomain(departmentId));

        return result.Match(
            m => CreatedAtAction(nameof(GetById), new { departmentId, id = m.Id }, m.ToContract()),
            Problem);
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Policy = SuperAdmin)]
    public async Task<IActionResult> Delete(int departmentId, int id)
    {
        var result = await majorService.DeleteAsync(departmentId, id);
        
        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Policy = SuperAdmin)]
    public async Task<IActionResult> Update(int departmentId, int id, UpdateMajorRequest request)
    {
        var result = await majorService.UpdateAsync(departmentId, id, request.ToDomain());
        
        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int departmentId, int id)
    {
        var result = await majorService.GetByIdAsync(departmentId, id);

        return result.Match(m => Ok(m.ToContract()), Problem);
    }

    [HttpPost("search")]
    public async Task<IActionResult> GetByDepartmentAsync(int departmentId, RetrievalRequest request)
    {
        var result = await majorService.GetByDepartmentAsync(departmentId, request.ToSieveModel());
        
        return result.Match(majors => Ok(majors.ToContractPaginatedList(MajorMapper.ToContract)), Problem);
    }
}