using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Departments.Requests;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Departments;

namespace MLMS.API.Departments;

[Authorize]
[Route("api/v1/departments")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class DepartmentController(IDepartmentService departmentService) : ApiControllerBase
{
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(CreateDepartmentRequest request)
    {
        var result = await departmentService.CreateAsync(request.ToDomain());

        return result.Match(d => CreatedAtAction(nameof(GetById), new { id = d.Id }, d.ToContract()), Problem);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await departmentService.DeleteAsync(id);
        
        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(int id, UpdateDepartmentRequest request)
    {
        var result = await departmentService.UpdateAsync(id, request.ToDomain());

        return result.Match(_ => NoContent(), Problem);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await departmentService.GetByIdAsync(id);

        return result.Match(d => Ok(d.ToContract()), Problem);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Get(RetrievalRequest request)
    {
        var result = await departmentService.GetAsync(request.ToSieveModel());
        
        return result.Match(departments => Ok(new PaginatedList<DepartmentResponse>
        {
            Items = departments.Items.Select(d => d.ToContract()).ToList(),
            Metadata = departments.Metadata
        }), Problem);
    }
}