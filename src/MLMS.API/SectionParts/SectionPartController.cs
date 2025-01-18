using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.SectionParts.Requests;
using MLMS.Domain.SectionParts;

using static MLMS.API.Common.AuthorizationPolicies;

namespace MLMS.API.SectionParts;

[Route("sections/{sectionId:long}/parts")]
[ApiVersion("1.0")]
public class SectionPartController(ISectionPartService sectionPartService) : ApiControllerBase
{
    [HttpPost]
    [Authorize(Policy = Admin)]
    public async Task<IActionResult> Create(long sectionId, CreateSectionPartRequest request)
    {
        var result = await sectionPartService.CreateAsync(request.ToDomain(sectionId));

        return result.Match(sp => CreatedAtAction(nameof(GetById), new { sectionId, id = sp.Id }, sp.ToContract()), Problem);
    }
    
    [HttpPut("{id:long}")]
    [Authorize(Policy = Admin)]
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
    [Authorize(Policy = Admin)]
    public async Task<IActionResult> Delete(long sectionId, long id)
    {
        var result = await sectionPartService.DeleteAsync(sectionId, id);
    
        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpPatch("{id:long}/current-user/done")]
    [Authorize(Policy = Staff)]
    public async Task<IActionResult> ToggleUserDoneStatus(long sectionId, long id)
    {
        var result = await sectionPartService.ToggleUserDoneStatusAsync(sectionId, id);
    
        return result.Match(_ => NoContent(), Problem);
    }
}