using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.SectionParts.Requests;
using MLMS.Domain.SectionParts;

namespace MLMS.API.SectionParts;

[Route("api/v1/sections/{sectionId:long}/parts")]
public class SectionPartController(ISectionPartService sectionPartService) : ApiControllerBase
{
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Admin)]
    public async Task<IActionResult> Create(long sectionId, CreateSectionPartRequest request)
    {
        var result = await sectionPartService.CreateAsync(request.ToDomain(sectionId));

        return result.Match(sp => CreatedAtAction(nameof(GetById), new { sectionId, id = sp.Id }, sp.ToContract()), Problem);
    }
    
    [HttpPut("{id:long}")]
    [Authorize(Policy = AuthorizationPolicies.Admin)]
    public async Task<IActionResult> Update(long sectionId, long id, UpdateSectionPartRequest request)
    {
        var result = await sectionPartService.UpdateAsync(id, request.ToDomain(sectionId));
    
        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<IActionResult> GetById(long sectionId, long id)
    {
        var result = await sectionPartService.GetByIdAsync(sectionId, id);

        return result.Match(sp => Ok(sp.ToContract()), Problem);
    }
    
    [HttpDelete("{id:long}")]
    [Authorize(Policy = AuthorizationPolicies.Admin)]
    public async Task<IActionResult> Delete(long sectionId, long id)
    {
        var result = await sectionPartService.DeleteAsync(sectionId, id);
    
        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpPut("{id:long}/exam/current-user/status")]
    [Authorize(Policy = AuthorizationPolicies.Staff)]
    public async Task<IActionResult> UpdateUserExamStatus(long sectionId, long id, UpdateStaffExamStatusRequest request)
    {
        var result = await sectionPartService.UpdateUserExamStatusAsync(sectionId, id, request.Answers);
    
        return result.Match(Ok, Problem);
    }
    
    [HttpPatch("{id:long}/current-user/done")]
    [Authorize(Policy = AuthorizationPolicies.Staff)]
    public async Task<IActionResult> ToggleUserDoneStatus(long sectionId, long id)
    {
        var result = await sectionPartService.ToggleUserDoneStatusAsync(sectionId, id);
    
        return result.Match(_ => NoContent(), Problem);
    }
}