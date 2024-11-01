using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Departments.Requests;
using MLMS.Domain.Departments;

namespace MLMS.API.Departments;

[Authorize]
[Route("v1/departments")]
public class DepartmentController(IDepartmentService departmentService) : ApiController
{
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
    public async Task<IActionResult> Create(CreateDepartmentRequest request)
    {
        var result = await departmentService.CreateAsync(request.ToDomain());

        return result.Match(d => CreatedAtAction(nameof(GetById), new { id = d.Id }, d.ToContract()), Problem);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await departmentService.DeleteAsync(id);
        
        return result.Match(_ => NoContent(), Problem);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await departmentService.GetByIdAsync(id);

        return result.Match(Ok, Problem);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await departmentService.GetAsync();
        
        return result.Match(Ok, Problem);
    }
}