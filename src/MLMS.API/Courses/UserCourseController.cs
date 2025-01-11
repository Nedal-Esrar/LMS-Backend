using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.Domain.Courses;

using static MLMS.API.Common.AuthorizationPolicies;

namespace MLMS.API.Courses;

[Route("api/v1/user/courses-status")]
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