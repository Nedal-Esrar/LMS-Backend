using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Courses.Requests;
using MLMS.API.Courses.Responses;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Courses;

namespace MLMS.API.Courses;

[Route("api/v1/courses")]
public class CourseController(ICourseService courseService) : ApiControllerBase
{
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Admin)]
    public async Task<IActionResult> Initialize(InitializeCourseRequest request)
    {
        var result = await courseService.InitializeAsync(request.ToDomain());
    
        return result.Match(c => CreatedAtAction(nameof(GetById), new { id = c.Id }, c.ToSimplifiedContract()), Problem);
    }
    
    [HttpPut("{id:long}")]
    [Authorize(Policy = AuthorizationPolicies.Admin)]
    public async Task<IActionResult> Update(long id, UpdateCourseRequest request)
    {
        var result = await courseService.UpdateAsync(id, request.ToDomain());
    
        return result.Match(_ => NoContent(), Problem);
    }

    [HttpDelete("{id:long}")]
    [Authorize(Policy = AuthorizationPolicies.Admin)]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await courseService.DeleteAsync(id);
        
        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPut("{id:long}/assignments")]
    [Authorize(Policy = AuthorizationPolicies.Admin)]
    public async Task<IActionResult> UpdateAssignments(long id, EditCourseAssignmentsRequest request)
    {
        var result = await courseService.EditAssignmentsAsync(id, request.Assignments);

        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpPost("{id:long}/start")]
    [Authorize(Policy = AuthorizationPolicies.Staff)]
    public async Task<IActionResult> Start(long id)
    {
        var result = await courseService.StartAsync(id);

        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPost("{id:long}/finish")]
    [Authorize(Policy = AuthorizationPolicies.Staff)]
    public async Task<IActionResult> Finish(long id)
    {
        var result = await courseService.FinishAsync(id);

        return result.Match(courseStatus => Ok(new { Status = courseStatus }), Problem);
    }
    
    [HttpGet("{id:long}/is-finished")]
    [Authorize(Policy = AuthorizationPolicies.Staff)]
    public async Task<IActionResult> CheckIfFinished(long id)
    {
        var result = await courseService.CheckIfFinishedAsync(id);

        return result.Match(status => Ok(new { status.IsFinished, status.FinishedAtUtc }), Problem);
    }

    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await courseService.GetByIdAsync(id);

        return result.Match(c => Ok(c.ToDetailedContract()), Problem);
    }
    
    [HttpPost("search")]
    [Authorize]
    public async Task<IActionResult> Get(RetrievalRequest request)
    {
        var result = await courseService.GetAsync(request.ToSieveModel());
        
        return result.Match(courses => Ok(new PaginatedList<CourseSimplifiedResponse>
        {
            Items = courses.Items.Select(d => d.ToSimplifiedContract()).ToList(),
            Metadata = courses.Metadata
        }), Problem);
    }
    
    [HttpGet("{id:long}/assignments")]
    [Authorize(Policy = AuthorizationPolicies.Admin)]
    public async Task<IActionResult> GetAssignments(long id)
    {
        var result = await courseService.GetAssignmentsByIdAsync(id);

        return result.Match(assignments => Ok(assignments.Select(a => a.ToContract()).ToList()), Problem);
    }
}