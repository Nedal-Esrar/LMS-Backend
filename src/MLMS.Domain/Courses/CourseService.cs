using ErrorOr;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Exceptions;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Email;
using MLMS.Domain.Exams;
using MLMS.Domain.Identity.Enums;
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
    IUserService userService,
    IEmailService emailService,
    INotificationService notificationService,
    ICourseAssignmentService courseAssignmentService,
    IDbTransactionProvider dbTransactionProvider,
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

        course.CreatedById = userContext.Id;
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

        if (userContext.Role == UserRole.Staff)
        {
            var isCourseAssignedToUserResult = await courseAssignmentService.IsAssignedToUserAsync(id, userContext.Id);

            if (isCourseAssignedToUserResult.IsError)
            {
                return isCourseAssignedToUserResult.Errors;
            }

            var isCourseAssignedToUser = isCourseAssignedToUserResult.Value;

            if (!isCourseAssignedToUser)
            {
                return CourseErrors.NotAssigned;
            }
        }

        if (userContext.Role == UserRole.SubAdmin &&
            !await courseRepository.IsCreatedByUserAsync(id, userContext.Id))
        {
            return CourseErrors.NotCreatedByUser;
        }

        if (userContext.Role == UserRole.Staff)
        {
            // In this case, finish the current sessions if they are due for exams in this course.
            var examIds = await courseRepository.GetExamIdsByIdAsync(id);
            
            foreach (var examId in examIds)
            {
                var isSessionDueResult = await examService.IsSessionDueAsync(userContext.Id, examId);

                if (isSessionDueResult.IsError)
                {
                    return isSessionDueResult.Errors;
                }

                var isSessionDue = isSessionDueResult.Value;
                
                if (isSessionDue)
                {
                    await examService.FinishCurrentSessionAsync(examId);
                }
            }
        }
        
        var course = await courseRepository.GetDetailedByIdAsync(id)!;
        
        course!.Sections.ForEach(
            s => s.SectionParts
                .Where(sp => sp.MaterialType == MaterialType.Exam)
                .ToList()
                .ForEach(sp =>
                {
                    if (sp.Exam != null) sp.Exam.MaxGradePoints = sp.Exam.Questions.Sum(q => q.Points);
                }));

        if (userContext.Role == UserRole.Staff)
        {
            // eliminate questions for exams.
            course!.Sections.ForEach(
                s => s.SectionParts
                    .Where(sp => sp.MaterialType == MaterialType.Exam)
                    .ToList()
                    .ForEach(sp =>
                    {
                        if (sp.Exam != null) sp.Exam.Questions = [];
                    }));
        }
        
        return course!;
    }

    public async Task<ErrorOr<None>> EditAssignmentsAsync(long id, List<int> newAssignments)
    {
        var course = await courseRepository.GetDetailedByIdAsync(id);
        
        if (course is null)
        {
            return CourseErrors.NotFound;
        }
        
        var newAssignmentsSet = newAssignments.ToHashSet();
        
        var oldAssignmentsResult = await courseAssignmentService.GetCourseAssignmentsByCourseIdAsync(id);

        if (oldAssignmentsResult.IsError)
        {
            return oldAssignmentsResult.Errors;
        }

        var oldAssignments = oldAssignmentsResult.Value;

        var oldAssignmentsSet = oldAssignments
            .Select(ca => ca.MajorId)
            .ToHashSet();

        var newAssignmentsToAdd = newAssignmentsSet.Where(a => !oldAssignmentsSet.Contains(a))
            .ToList();
        
        var oldAssignmentsToRemove = oldAssignmentsSet.Where(a => !newAssignmentsSet.Contains(a))
            .ToList();

        var newUsersToAssignResult = await userService.GetByMajorsAsync(newAssignmentsToAdd);

        if (newUsersToAssignResult.IsError)
        {
            return newUsersToAssignResult.Errors;
        }

        var newUsersToAssign = newUsersToAssignResult.Value;
        
        var oldUsersToUnassignResult = await userService.GetByMajorsAsync(oldAssignmentsToRemove);

        if (oldUsersToUnassignResult.IsError)
        {
            return oldUsersToUnassignResult.Errors;
        }

        var oldUsersToUnassign = oldUsersToUnassignResult.Value;
        
        var newUserCourseRelationships = newUsersToAssign.Select(u => new UserCourse
        {
            CourseId = id,
            UserId = u.Id,
            Status = UserCourseStatus.NotStarted
        }).ToList();

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            var courseAssignmentCreationResult = await courseAssignmentService.CreateCourseAssignmentsAsync(
                newAssignmentsToAdd.Select(m => new CourseAssignment
                {
                    CourseId = id,
                    MajorId = m
                }).ToList());

            if (courseAssignmentCreationResult.IsError)
            {
                throw new UnoccasionalErrorException(courseAssignmentCreationResult.Errors);
            }

            var userCoursesCreationResult =
                await courseAssignmentService.CreateUserCoursesAsync(newUserCourseRelationships);

            if (userCoursesCreationResult.IsError)
            {
                throw new UnoccasionalErrorException(userCoursesCreationResult.Errors);
            }
            
            await notificationService.CreateAsync(newUsersToAssign.Select(u => new Notification
            {
                UserId = u.Id,
                Content = $"The course {course.Name} has been assigned to you.",
                CreatedAtUtc = DateTime.UtcNow,
                Title = "New course assigned to you."
            }).ToList());

            var courseAssignmentDeletionResult = await courseAssignmentService.DeleteCourseAssignmentsAsync(
                oldAssignmentsToRemove.Select(m => new CourseAssignment
                {
                    CourseId = id,
                    MajorId = m
                }).ToList());

            if (courseAssignmentDeletionResult.IsError)
            {
                throw new UnoccasionalErrorException(courseAssignmentDeletionResult.Errors);
            }

            var userCourseDeletionResult =
                await courseAssignmentService.DeleteUserCoursesAsync(id, oldUsersToUnassign.Select(u => u.Id).ToList());

            if (userCourseDeletionResult.IsError)
            {
                throw new UnoccasionalErrorException(userCourseDeletionResult.Errors);
            }

            await notificationService.CreateAsync(oldUsersToUnassign.Select(u => new Notification
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
                    BodyHtml = EmailUtils.GetAssignmentEmailBody(course.Name)
                });
            }

            if (oldUsersToUnassign.Count != 0)
            {
                await emailService.SendAsync(new EmailRequest
                {
                    ToEmails = oldUsersToUnassign.Select(u => u.Email).ToList(),
                    Subject = "New course assigned to you.",
                    BodyHtml = EmailUtils.GetUnassignmentEmailBody(course.Name)
                });
            }
        });
        
        return None.Value;
    }

    public async Task<ErrorOr<PaginatedList<Course>>> GetAsync(SieveModel sieveModel)
    {
        var userId = userContext.Id;
        
        return userContext.Role switch
        {
            UserRole.Admin => await courseRepository.GetAsync(sieveModel),
            UserRole.Staff => await courseRepository.GetAssignedByUserId(userId, sieveModel),
            UserRole.SubAdmin => await courseRepository.GetCreatedByUserId(userId, sieveModel),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public async Task<ErrorOr<UserCourseStatus>> FinishAsync(long id)
    {
        if (!await courseRepository.ExistsByIdAsync(id))
        {
            return CourseErrors.NotFound;
        }
        
        var userId = userContext.Id;
        
        var userCourseResult = await courseAssignmentService.GetUserCourseAsync(id, userId);
        
        if (userCourseResult.IsError)
        {
            return userCourseResult.Errors;
        }
        
        var userCourse = userCourseResult.Value;

        if (userCourse.Status != UserCourseStatus.InProgress && userCourse.Status != UserCourseStatus.Failed)
        {
            return CourseErrors.NotInProgress;
        }
        
        var examStatusesResult = await examService.GetExamStatusesByCourseAndUserAsync(id, userId);

        if (examStatusesResult.IsError)
        {
            return examStatusesResult.Errors;
        }
        
        var examStatuses = examStatusesResult.Value;

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

        await courseAssignmentService.UpdateUserCoursesAsync([userCourse]);
        
        return courseStatus;
    }

    public async Task<ErrorOr<(bool IsFinished, DateTime? FinishedAtUtc)>> CheckIfFinishedAsync(long id)
    {
        var userCourseResult = await courseAssignmentService.GetUserCourseAsync(id, userContext.Id);

        if (userCourseResult.IsError)
        {
            return userCourseResult.Errors;
        }

        var userCourse = userCourseResult.Value;

        return (userCourse.Status == UserCourseStatus.Finished, userCourse.FinishedAtUtc);
    }

    public async Task<ErrorOr<None>> StartAsync(long id)
    {
        if (!await courseRepository.ExistsByIdAsync(id))
        {
            return CourseErrors.NotFound;
        }
        
        var userId = userContext.Id;
        
        var userCourseResult = await courseAssignmentService.GetUserCourseAsync(id, userId);

        if (userCourseResult.IsError)
        {
            return userCourseResult.Errors;
        }
        
        var userCourse = userCourseResult.Value;

        if (userCourse.Status != UserCourseStatus.NotStarted)
        {
            return CourseErrors.AlreadyStarted;
        }
        
        userCourse.Status = UserCourseStatus.InProgress;
        userCourse.StartedAtUtc = DateTime.UtcNow;
        userCourse.FinishedAtUtc = null;
        
        await courseAssignmentService.UpdateUserCoursesAsync([userCourse]);

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

        return await courseAssignmentService.GetCourseAssignmentsByCourseIdAsync(id, includeMajorAssignments: true);
    }

    public async Task<ErrorOr<Dictionary<UserCourseStatus, int>>> GetCourseStatusForCurrentUserAsync()
    {
        var userId = userContext.Id;
        
        var userCourseStatusesResult = await courseAssignmentService.GetUserCoursesByUserIdAsync(userId);

        if (userCourseStatusesResult.IsError)
        {
            return userCourseStatusesResult.Errors;
        }

        var userCourseStatuses = userCourseStatusesResult.Value;

        var statusCountMap = userCourseStatuses
            .GroupBy(uc => uc.Status)
            .ToDictionary(g => g.Key, g => g.Count());

        return Enum.GetValues<UserCourseStatus>()
            .ToDictionary(ev => ev, ev => statusCountMap.GetValueOrDefault(ev, 0));
    }
    
    public async Task<ErrorOr<PaginatedList<UserCourse>>> GetParticipantsByIdAsync(long courseId, SieveModel sieveModel)
    {
        return await courseAssignmentService.GetUserCourseByCourseIdAsync(courseId, userContext.Role == UserRole.SubAdmin, sieveModel);
    }

    public async Task<ErrorOr<None>> NotifyParticipantAsync(long id, int userId)
    {
        var course = await courseRepository.GetByIdAsync(id);

        if (course is null)
        {
            return CourseErrors.NotFound;
        }

        var userRetrievalResult = await userService.GetByIdAsync(userId);

        if (userRetrievalResult.IsError)
        {
            return userRetrievalResult.Errors;
        }

        var user = userRetrievalResult.Value;

        var userAssignmentResult = await courseAssignmentService.IsAssignedToUserAsync(id, userId);

        if (userAssignmentResult.IsError)
        {
            return userAssignmentResult.Errors;
        }

        var isAssigned = userAssignmentResult.Value;

        if (!isAssigned)
        {
            return CourseErrors.NotAssigned;
        }

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            // create a notification.
            await notificationService.CreateAsync([
                new Notification
                {
                    UserId = userId,
                    Title = "Course poke",
                    Content = $"You have been poked for the course {course.Name}.",
                    CreatedAtUtc = DateTime.UtcNow
                }
            ]);
            
            // send an email.
            await emailService.SendAsync(new EmailRequest
            {
                ToEmails = [user.Email],
                Subject = "Course poke",
                BodyHtml = EmailUtils.GetCoursePokeEmailBody(course.Name)
            });
        });

        return None.Value;
    }

    public async Task<ErrorOr<None>> ChangeManagerAsync(long id, int subAdminId)
    {
        if (!await courseRepository.ExistsByIdAsync(id))
        {
            return CourseErrors.NotFound;
        }

        var userExistenceResult = await userService.CheckExistenceByIdAsync(subAdminId);

        if (userExistenceResult.IsError)
        {
            return userExistenceResult.Errors;
        }

        var userIsSubAdminResult = await userService.CheckIfSubAdminAsync(subAdminId);
        
        if (userIsSubAdminResult.IsError)
        {
            return userIsSubAdminResult.Errors;
        }
        
        await courseRepository.ChangeManagerAsync(id, subAdminId);

        return None.Value;
    }

    public async Task<ErrorOr<None>> CheckExistenceAsync(long id)
    {
        return await courseRepository.ExistsByIdAsync(id) ? 
            None.Value :
            CourseErrors.NotFound;
    }
}