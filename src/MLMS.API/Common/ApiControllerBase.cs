using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MLMS.API.Common;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        if (errors.Any(e => e.Type != ErrorType.Validation))
        {
            return ConstructProblem(errors);
        }

        return ConstructValidationProblem(errors);
    }
    
    private IActionResult ConstructValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();
        
        errors.ForEach(e => modelStateDictionary.AddModelError(e.Code, e.Description));

        return ValidationProblem(modelStateDictionary);
    }

    private IActionResult ConstructProblem(List<Error> errors)
    {
        HttpContext.Items.Add("errors", errors);

        var errorToConsider = errors.First();
        
        var statusCode = errorToConsider.Type switch
        {
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unexpected => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var detail = errorToConsider.Metadata is { Count: > 0 }
            ? errorToConsider.Metadata.Values.First()?.ToString()
            : null;

        return Problem(
            statusCode: statusCode,
            title: errorToConsider.Description,
            detail: detail);
    }
}