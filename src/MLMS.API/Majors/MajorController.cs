using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Departments.Requests;
using MLMS.API.Majors.Requests;
using MLMS.Domain.Majors;

namespace MLMS.API.Majors;

[Authorize]
[Route("api/v1/departments/{departmentId:int}/majors")]
public class MajorController(IMajorService majorService) : ApiController
{
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
    public async Task<IActionResult> Create(int departmentId, CreateMajorRequest request)
    {
        var result = await majorService.CreateAsync(request.ToDomain(departmentId));

        return result.Match(
            m => CreatedAtAction(nameof(GetById), new { departmentId, id = m.Id }, m.ToContract()),
            Problem);
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
    public async Task<IActionResult> Delete(int departmentId, int id)
    {
        var result = await majorService.DeleteAsync(departmentId, id);
        
        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int departmentId, int id)
    {
        var result = await majorService.GetByIdAsync(departmentId, id);

        return result.Match(Ok, Problem);
    }

    [HttpGet]
    public async Task<IActionResult> GetByDepartmentAsync(int departmentId)
    {
        var result = await majorService.GetByDepartmentAsync(departmentId);

        return result.Match(Ok, Problem);
    }
}