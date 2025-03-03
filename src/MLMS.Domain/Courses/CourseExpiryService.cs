using ErrorOr;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Email;
using MLMS.Domain.Exams;
using MLMS.Domain.Notifications;
using MLMS.Domain.SectionParts;
using MLMS.Domain.UsersCourses;

namespace MLMS.Domain.Courses;

public class CourseExpiryService(
    IDbTransactionProvider dbTransactionProvider,
    ICourseAssignmentService courseAssignmentService,
    INotificationService notificationService,
    IEmailService emailService, 
    ISectionPartService sectionPartService,
    IExamService examService
    ) : ICourseExpiryService
{
    public async Task<ErrorOr<None>> CheckCoursesExpiryAsync()
    {
        // TODO: For now, process everything as a whole, in the future, make batches.
        // Retrieve user course states (finished ones with expiration enabled for the course).
        // filter them on whether the relation is expired or not
        // for the expired ones, reset the done state, the exam states, the finished at date, and the course state.
        // and send an email and a notification to users that their course has expired.
        var userCourses = (await courseAssignmentService.GetStatesForFinishedCoursesWithExpirationAsync()).Value;

        var now = DateTime.UtcNow;
        
        var expiredUserCourses = userCourses
            .Where(uc => uc.FinishedAtUtc!.Value.AddMonths(uc.Course.ExpirationMonths!.Value) < now)
            .ToList();

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {   
            expiredUserCourses.ForEach(uc => uc.Status = UserCourseStatus.Expired);

            await courseAssignmentService.UpdateUserCoursesAsync(expiredUserCourses);
            
            // reset done states
            await sectionPartService.ResetDoneStatesByUserCoursesAsync(expiredUserCourses);
            
            // reset exam states
            await examService.ResetExamStatesByUserCoursesAsync(expiredUserCourses);
            
            // create notifications
            var expirationNotificationsToCreate = expiredUserCourses.Select(uc => new Notification
            {
                Title = $"Course {uc.Course.Name} has expired",
                Content = $"The course {uc.Course.Name} has expired. it has passed the time of {uc.Course.ExpirationMonths} months or more since started, please consider retaking it retake it.",
                CreatedAtUtc = now,
                UserId = uc.UserId
            }).ToList();

            await notificationService.CreateAsync(expirationNotificationsToCreate);

            // send emails
            var userCoursesByCourseId = userCourses.GroupBy(uc => uc.Course)
                .ToDictionary(uc => uc.Key, uc => uc.ToList());
            
            var sendEmailTasks = userCoursesByCourseId
                .Select(ucs => Task.Run(() =>
                {
                    var course = ucs.Key;
            
                    var emailsToSend = ucs.Value.Select(uc => uc.User.Email)
                        .ToList();
            
                    return emailService.SendAsync(new EmailRequest
                    {
                        ToEmails = emailsToSend,
                        Subject = $"Course {course.Name} has expired",
                        BodyHtml = EmailUtils.GetCourseExpiredEmailBody(course.Name, course.ExpirationMonths!.Value)
                    });
                }));
            
            await Task.WhenAll(sendEmailTasks);
        });
        
        return None.Value;
    }
}