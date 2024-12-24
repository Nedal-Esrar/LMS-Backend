using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Sections.Requests;
using MLMS.Domain.Sections;

namespace MLMS.API.Sections;

[Route("api/v1/courses/{courseId:long}/sections")]
[Authorize(Policy = AuthorizationPolicies.Admin)]
public class SectionController(ISectionService sectionService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(long courseId, CreateSectionRequest request)
    {
        var result = await sectionService.CreateAsync(request.ToDomain(courseId));
    
        return result.Match(s => CreatedAtAction(nameof(GetById), new { courseId, id = s.Id }, s.ToContract()), Problem);
    }
    
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long courseId, long id, UpdateSectionRequest request)
    {
        var result = await sectionService.UpdateAsync(id, request.ToDomain(courseId));

        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<IActionResult> GetById(long courseId, long id)
    {
        var result = await sectionService.GetByIdAsync(courseId, id);

        return result.Match(s => Ok(s.ToContract()), Problem);
    }
    
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long courseId, long id)
    {
        var result = await sectionService.DeleteAsync(courseId, id);

        return result.Match(_ => NoContent(), Problem);
    }
}