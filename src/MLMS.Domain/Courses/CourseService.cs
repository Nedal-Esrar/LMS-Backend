using ErrorOr;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Exams;
using MLMS.Domain.Identity;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Notifications;
using MLMS.Domain.SectionParts;
using MLMS.Domain.Users;
using MLMS.Domain.UsersCourses;
using Sieve.Models;

namespace MLMS.Domain.Courses;

public class CourseService(
    ICourseRepository courseRepository,
    IUserContext userContext,
    IUserRepository userRepository,
    IEmailService emailService,
    INotificationRepository notificationRepository,
    ICourseAssignmentRepository courseAssignmentRepository,
    IDbTransactionProvider dbTransactionProvider,
    IUserCourseRepository userCourseRepository,
    ISectionPartRepository sectionPartRepository,
    IExamRepository examRepository,
    IExamService examService) : ICourseService
{
    private readonly CourseValidator _courseValidator = new();
    
    public async Task<ErrorOr<Course>> InitializeAsync(Course course)
    {
        var validationResult = await _courseValidator.ValidateAsync(course);

        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }

        if (await courseRepository.ExistsByNameAsync(course.Name))
        {
            return CourseErrors.NameAlreadyExists;
        }

        course.CreatedById = userContext.Id!.Value;
        course.CreatedAtUtc = DateTime.UtcNow;

        var createdCourse = await courseRepository.CreateAsync(course);

        return createdCourse;
    }

    public async Task<ErrorOr<Course>> GetByIdAsync(long id)
    {
        if (!await courseRepository.ExistsByIdAsync(id))
        {
            return CourseErrors.NotFound;
        }

        if (userContext.Role == UserRole.Staff &&
            !await userCourseRepository.ExistsByCourseAndUserIdsAsync(id, userContext.Id!.Value))
        {
            return CourseErrors.NotAssigned;
        }

        if (userContext.Role == UserRole.SubAdmin &&
            !await courseRepository.IsCreatedByUserAsync(id, userContext.Id!.Value))
        {
            return CourseErrors.NotCreatedByUser;
        }

        var course = await courseRepository.GetDetailedByIdAsync(id)!;

        if (userContext.Role != UserRole.Staff)
        {
            return course!;
        }
        
        // Finish exam sessions that are not finished explicitly and expired.
        var examIds = course!.Sections
            .SelectMany(s => s.SectionParts)
            .Where(sp => sp.MaterialType == MaterialType.Exam)
            .Select(sp => sp.ExamId!.Value);

        foreach (var examId in examIds)
        {
            if (await examRepository.IsSessionDueAsync(userContext.Id!.Value, examId))
            {
                await examService.FinishCurrentSessionAsync(examId);
            }
        }

        return course!;
    }

    public async Task<ErrorOr<None>> EditAssignmentsAsync(long id, List<int> newAssignments)
    {
        // TODO: UserSectionPartDone
        var course = await courseRepository.GetDetailedByIdAsync(id);
        
        if (course is null)
        {
            return CourseErrors.NotFound;
        }
        
        var newAssignmentsSet = newAssignments.ToHashSet();

        var oldAssignmentsSet = (await courseAssignmentRepository.GetByCourseIdAsync(id))
            .Select(ca => ca.MajorId)
            .ToHashSet();

        var newAssignmentsToAdd = newAssignmentsSet.Where(a => !oldAssignmentsSet.Contains(a))
            .ToList();
        
        var oldAssignmentsToRemove = oldAssignmentsSet.Where(a => !newAssignmentsSet.Contains(a))
            .ToList();

        var newUsersToAssign = await userRepository.GetByMajorsAsync(newAssignmentsToAdd);
        var oldUsersToUnassign = await userRepository.GetByMajorsAsync(oldAssignmentsToRemove);
        
        var newUserCourseRelationships = newUsersToAssign.Select(u => new UserCourse
        {
            CourseId = id,
            UserId = u.Id,
            Status = UserCourseStatus.NotStarted
        }).ToList();

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            await courseAssignmentRepository.CreateAsync(newAssignmentsToAdd.Select(m => new CourseAssignment
                {
                    CourseId = id,
                    MajorId = m
                }).ToList());
            
            await userCourseRepository.CreateAsync(newUserCourseRelationships);
            
            await notificationRepository.CreateAsync(newUsersToAssign.Select(u => new Notification
            {
                UserId = u.Id,
                Content = $"The course {course.Name} has been assigned to you.",
                CreatedAtUtc = DateTime.UtcNow,
                Title = "New course assigned to you."
            }).ToList());
            
            await courseAssignmentRepository.DeleteAsync(oldAssignmentsToRemove.Select(m => new CourseAssignment
            {
                CourseId = id,
                MajorId = m
            }).ToList());
            
            await userCourseRepository.DeleteByCourseAndUserIdsAsync(id, oldUsersToUnassign.Select(u => u.Id).ToList());

            await notificationRepository.CreateAsync(oldUsersToUnassign.Select(u => new Notification
            {
                UserId = u.Id,
                Content = $"The course {course.Name} has been unassigned from you.",
                CreatedAtUtc = DateTime.UtcNow,
                Title = "A course has been unassigned from you."
            }).ToList());

            if (newUsersToAssign.Count != 0)
            {
                await emailService.SendAsync(new EmailRequest
                {
                    ToEmails = newUsersToAssign.Select(u => u.Email).ToList(),
                    Subject = "New course assigned to you.",
                    Body = EmailUtils.GetAssignmentEmailBody(course.Name)
                });
            }

            if (oldUsersToUnassign.Count != 0)
            {
                await emailService.SendAsync(new EmailRequest
                {
                    ToEmails = oldUsersToUnassign.Select(u => u.Email).ToList(),
                    Subject = "New course assigned to you.",
                    Body = EmailUtils.GetUnassignmentEmailBody(course.Name)
                });
            }
        });
        
        return None.Value;
    }

    public async Task<ErrorOr<PaginatedList<Course>>> GetAsync(SieveModel sieveModel)
    {
        var userId = userContext.Id!.Value;
        
        return userContext.Role switch
        {
            UserRole.Admin => await courseRepository.GetAsync(sieveModel),
            UserRole.Staff => await courseRepository.GetAssignedByUserId(userId, sieveModel),
            UserRole.SubAdmin => await courseRepository.GetCreatedByUserId(userId, sieveModel)
        };
    }

    public async Task<ErrorOr<UserCourseStatus>> FinishAsync(long id)
    {
        if (!await courseRepository.ExistsByIdAsync(id))
        {
            return CourseErrors.NotFound;
        }
        
        var userId = userContext.Id!.Value;
        
        var userCourse = await userCourseRepository.GetByUserAndCourseAsync(id, userId);

        if (userCourse.Status != UserCourseStatus.InProgress)
        {
            return CourseErrors.NotInProgress;
        }
        
        var examStatuses = await sectionPartRepository.GetExamStatusesByCourseAndUserAsync(id, userId);

        if (examStatuses.Any(es => es.Status == ExamStatus.NotTaken))
        {
            return CourseErrors.StillInProgress;
        }
        
        var courseStatus = examStatuses.Any(es => es.Status == ExamStatus.Failed)
            ? UserCourseStatus.Failed
            : UserCourseStatus.Finished;
        
        userCourse.Status = courseStatus;

        if (courseStatus == UserCourseStatus.Finished)
        {
            userCourse.FinishedAtUtc = DateTime.UtcNow;
        }

        await userCourseRepository.UpdateAsync(userCourse);
        
        return courseStatus;
    }

    public async Task<ErrorOr<(bool IsFinished, DateTime? FinishedAtUtc)>> CheckIfFinishedAsync(long id)
    {
        var userCourse = await userCourseRepository.GetByUserAndCourseAsync(id, userContext.Id!.Value);

        return (userCourse.Status == UserCourseStatus.Finished, userCourse.FinishedAtUtc);
    }

    public async Task<ErrorOr<None>> StartAsync(long id)
    {
        if (!await courseRepository.ExistsByIdAsync(id))
        {
            return CourseErrors.NotFound;
        }
        
        var userId = userContext.Id!.Value;
        
        var userCourse = await userCourseRepository.GetByUserAndCourseAsync(id, userId);

        if (userCourse.Status != UserCourseStatus.NotStarted)
        {
            return CourseErrors.AlreadyStarted;
        }
        
        userCourse.Status = UserCourseStatus.InProgress;
        userCourse.StartedAtUtc = DateTime.UtcNow;
        
        await userCourseRepository.UpdateAsync(userCourse);

        return None.Value;
    }

    public async Task<ErrorOr<None>> UpdateAsync(long id, Course course)
    {
        var validationResult = await _courseValidator.ValidateAsync(course);
        
        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }
        
        var existingCourse = await courseRepository.GetByIdAsync(id);

        if (existingCourse is null)
        {
            return CourseErrors.NotFound;
        }
        
        if (existingCourse.Name != course.Name && await courseRepository.ExistsByNameAsync(course.Name))
        {
            return CourseErrors.NameAlreadyExists;
        }
        
        existingCourse.MapForUpdate(course);
        
        await courseRepository.UpdateAsync(existingCourse);

        return None.Value;
    }

    public async Task<ErrorOr<None>> DeleteAsync(long id)
    {
        if (!await courseRepository.ExistsByIdAsync(id))
        {
            return CourseErrors.NotFound;
        }

        await courseRepository.DeleteAsync(id);

        return None.Value;
    }

    public async Task<ErrorOr<List<CourseAssignment>>> GetAssignmentsByIdAsync(long id)
    {
        if (!await courseRepository.ExistsByIdAsync(id))
        {
            return CourseErrors.NotFound;
        }

        return await courseAssignmentRepository.GetByCourseIdAsync(id, includeMajorAssignments: true);
    }

    public async Task<ErrorOr<Dictionary<UserCourseStatus, int>>> GetCourseStatusForCurrentUserAsync()
    {
        var userId = userContext.Id!.Value;
        
        var userCourseStatuses = await userCourseRepository.GetByUserIdAsync(userId);

        var statusCountMap = userCourseStatuses
            .GroupBy(uc => uc.Status)
            .ToDictionary(g => g.Key, g => g.Count());

        return Enum.GetValues<UserCourseStatus>()
            .ToDictionary(ev => ev, ev => statusCountMap.GetValueOrDefault(ev, 0));
    }

    public async Task<ErrorOr<None>> CheckCoursesExpiryAsync()
    {
        // TODO: for now, process everything as a whole, in the future, make batches.
        // Retrieve user course states (finished ones with expiration enabled for the course).
        // filter them on whether the relation is expired or not
        // for the expired ones, reset the done state, the exam states, the finished at date, and the course state.
        // and send an email and a notification to users that their course has expired.
        
        var userCourses = await userCourseRepository.GetStatesForFinishedCoursesWithExpirationAsync();

        var now = DateTime.UtcNow;
        
        var expiredUserCourses = userCourses
            .Where(uc => uc.FinishedAtUtc!.Value.AddMonths(uc.Course.ExpirationMonths!.Value) < now)
            .ToList();

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {   
            expiredUserCourses.ForEach(uc =>
            {
                uc.FinishedAtUtc = null;
                uc.Status = UserCourseStatus.Expired;
            });

            await userCourseRepository.UpdateAsync(expiredUserCourses);
            
            // reset done states
            await sectionPartRepository.ResetDoneStatesByUserCoursesAsync(expiredUserCourses);
            
            // reset exam states
            await examRepository.ResetExamStatesByUserCoursesAsync(expiredUserCourses);
            
            // create notifications
            var expirationNotificationsToCreate = expiredUserCourses.Select(uc => new Notification
            {
                Title = $"Course {uc.Course.Name} has expired",
                Content = $"The course {uc.Course.Name} has expired. it has passed the time of {uc.Course.ExpirationMonths} months or more since started, please consider retaking it retake it.",
                UserId = uc.UserId
            }).ToList();

            await notificationRepository.CreateAsync(expirationNotificationsToCreate);

            // create emails
            // var userCoursesByCourseId = userCourses.GroupBy(uc => uc.Course)
            //     .ToDictionary(uc => uc.Key, uc => uc.ToList());
            //
            // var sendEmailTasks = userCoursesByCourseId
            //     .Select(ucs => Task.Run(() =>
            //     {
            //         var course = ucs.Key;
            //
            //         var emailsToSend = ucs.Value.Select(uc => uc.User.Email)
            //             .ToList();
            //
            //         return emailService.SendAsync(new EmailRequest
            //         {
            //             ToEmails = emailsToSend,
            //             Subject = $"Course {course.Name} has expired",
            //             // Body = EmailUtils.GetCourseExpiredEmailBody(course.Name)
            //         });
            //     }));
            //
            // await Task.WhenAll(sendEmailTasks);
        });
        
        return None.Value;
    }

    public async Task<ErrorOr<PaginatedList<UserCourse>>> GetParticipantsByIdAsync(long courseId, SieveModel sieveModel)
    {
        return await userCourseRepository.GetByCourseIdAsync(courseId, userContext.Role == UserRole.SubAdmin, sieveModel);
    }

    public async Task<ErrorOr<None>> NotifyParticipantAsync(long id, int userId)
    {
        var course = await courseRepository.GetByIdAsync(id);

        if (course is null)
        {
            return CourseErrors.NotFound;
        }

        var user = await userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return UserErrors.NotFound;
        }

        if (!await userCourseRepository.ExistsByCourseAndUserIdsAsync(id, userId))
        {
            return CourseErrors.NotAssigned;
        }

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            // create a notification.
            // await notificationRepository.CreateAsync([
            //     new Notification
            //     {
            //         
            //     }
            // ]);
            
            // send an email.
        });

        return None.Value;
    }
}