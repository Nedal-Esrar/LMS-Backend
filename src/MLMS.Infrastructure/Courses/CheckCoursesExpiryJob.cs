using Microsoft.Extensions.Logging;
using MLMS.Domain.Courses;
using Quartz;

namespace MLMS.Infrastructure.Courses;

[DisallowConcurrentExecution]
public class CheckCoursesExpiryJob(
    ICourseService courseService,
    ILogger<CheckCoursesExpiryJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await courseService.CheckCoursesExpiryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while checking courses expiry");
        }
    }
}