using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.Domain.Courses;

using static MLMS.API.Common.AuthorizationPolicies;

namespace MLMS.API.Courses;

[Route("user/courses-status")]
[ApiVersion("1.0")]
[Authorize(Policy = Staff)]
public class UserCourseController(ICourseService courseService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCoursesStatus()
    {
        var result = await courseService.GetCourseStatusForCurrentUserAsync();
        
        return result.Match(Ok, Problem);
    }
}