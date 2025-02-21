using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Exams.Requests;
using MLMS.API.Exams.Responses;
using MLMS.Domain.Exams;

using static MLMS.API.Common.AuthorizationPolicies;

namespace MLMS.API.Exams;

[Route("exams/{examId:long}/user/current-session")]
[ApiVersion("1.0")]
[Authorize(Policy = Staff)]
public class ExamSessionController(IExamService examService) : ApiControllerBase
{
    [HttpPost("start")]
    public async Task<IActionResult> Start(long examId)
    {
        var result = await examService.StartSessionAsync(examId);

        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpGet("is-started")]
    public async Task<IActionResult> IsStarted(long examId)
    {
        var result = await examService.IsSessionStartedAsync(examId);

        return result.Match(isStarted => Ok(new { isStarted }), Problem);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ExamSessionStateResponse), 200)]
    public async Task<IActionResult> GetSessionDetails(long examId)
    {
        var result = await examService.GetSessionDetailsAsync(examId);

        return result.Match(details => Ok(details.ToContract()), Problem);
    }
    
    [HttpGet("questions/{questionId:long}")]
    [ProducesResponseType(typeof(ExamSessionQuestionChoiceResponse), 200)]
    public async Task<IActionResult> GetQuestion(long examId, long questionId)
    {
        var result = await examService.GetQuestionAsync(examId, questionId);

        return result.Match(q => Ok(q.ToContract()), Problem);
    }
    
    [HttpPut("questions/{questionId:long}/answer")]
    public async Task<IActionResult> AnswerQuestion(long examId, long questionId, AnswerQuestionRequest contract)
    {
        var result = await examService.AnswerQuestionAsync(examId, questionId, contract.Answer);

        return result.Match(_ => NoContent(), Problem);
    }
    
    [HttpPost("finish")]
    public async Task<IActionResult> Finish(long examId)
    {
        var result = await examService.FinishCurrentSessionAsync(examId);

        return result.Match(status => Ok(new { status }), Problem);
    }
}