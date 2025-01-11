using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.Domain.Exams;

using static MLMS.API.Common.AuthorizationPolicies;

namespace MLMS.API.Exams;

[Route("api/v1/exams")]
[Authorize(Policy = Staff)]
public class ExamController(IExamService examService) : ApiControllerBase
{
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var exam = await examService.GetByIdAsync(id);

        return exam.Match(e => Ok(e.ToSimpleContract()), Problem);
    }
}