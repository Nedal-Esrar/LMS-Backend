using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace MLMS.Infrastructure.Courses;

internal static class CoursesExpiryJobConfiguration
{
    public static void ConfigureCoursesExpiryJob(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            var jobKey = JobKey.Create(nameof(CheckCoursesExpiryJob));

            options.AddJob<CheckCoursesExpiryJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithCronSchedule("0 0 1,15 * * ?"));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }
}