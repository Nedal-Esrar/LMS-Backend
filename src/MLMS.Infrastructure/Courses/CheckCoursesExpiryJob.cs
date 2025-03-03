using Microsoft.Extensions.Logging;
using MLMS.Domain.Courses;
using Quartz;

namespace MLMS.Infrastructure.Courses;

[DisallowConcurrentExecution]
public class CheckCoursesExpiryJob(
    ICourseExpiryService courseExpiryService,
    ILogger<CheckCoursesExpiryJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await courseExpiryService.CheckCoursesExpiryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while checking courses expiry");
        }
    }
}