using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace MLMS.API.Common.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError("An unhandled exception has occurred: {Exception}", exception);

        await Results.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            title: "Internal server error",
            detail: "An internal error on the server occured.",
            extensions: new Dictionary<string, object?>
            {
                ["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier
            }).ExecuteAsync(httpContext);

        return true;
    }
}