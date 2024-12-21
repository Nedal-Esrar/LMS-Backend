using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Courses.Requests;
using MLMS.Domain.Courses;

namespace MLMS.API.Courses;

[Route("api/v1/courses")]
public class CourseController(ICourseService courseService) : ApiControllerBase
{
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.SubAdmin)]
    public async Task<IActionResult> Initialize(InitializeCourseRequest request)
    {
        var result = await courseService.InitializeAsync(request.ToDomain());
    
        return result.Match(c => CreatedAtAction(nameof(GetById), new { id = c.Id }), Problem);
    }

    [HttpPut("{id:long}/assignments")]
    [Authorize(Policy = AuthorizationPolicies.SubAdmin)]
    public async Task<IActionResult> UpdateAssignments(long id, EditCourseAssignmentsRequest request)
    {
        var result = await courseService.EditAssignmentsAsync(id, request.Assignments);

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

        return result.Match(isFinished => Ok(new { isFinished }), Problem);
    }

    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await courseService.GetByIdAsync(id);

        return Ok();

        // return result.Match(c => Ok(c.ToDetailedContracted()), Problem);
    }
    
    [HttpPost("search")]
    [Authorize]
    public async Task<IActionResult> Get(RetrievalRequest request)
    {
        var result = await courseService.GetAsync(request.ToSieveModel());

        return Ok();

        // return result.Match(courses => , Problem);
    }
}