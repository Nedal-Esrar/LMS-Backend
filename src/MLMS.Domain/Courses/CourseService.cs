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
    ISectionPartRepository sectionPartRepository) : ICourseService
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

        return userContext.Role switch
        {
            UserRole.Staff when !await userCourseRepository.ExistsByCourseAndUserIdsAsync(id, userContext.Id!.Value) =>
                CourseErrors.NotAssigned,
            UserRole.SubAdmin when !await courseRepository.IsCreatedByUserAsync(id, userContext.Id!.Value) =>
                CourseErrors.NotCreatedByUser,
            _ => await courseRepository.GetDetailedByIdAsync(id)!
        };
    }

    public async Task<ErrorOr<None>> EditAssignmentsAsync(long id, List<int> newAssignments)
    {
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
            await userCourseRepository.CreateAsync(newUserCourseRelationships);
            
            await notificationRepository.CreateAsync(newUsersToAssign.Select(u => new Notification
            {
                UserId = u.Id,
                Content = $"The course {course.Name} has been assigned to you.",
                CreatedAtUtc = DateTime.UtcNow,
                Title = "New course assigned to you."
            }).ToList());
            
            await userCourseRepository.DeleteByCourseAndUserIdsAsync(id, oldUsersToUnassign.Select(u => u.Id).ToList());

            await notificationRepository.CreateAsync(oldUsersToUnassign.Select(u => new Notification
            {
                UserId = u.Id,
                Content = $"The course {course.Name} has been unassigned from you.",
                CreatedAtUtc = DateTime.UtcNow,
                Title = "A course has been unassigned from you."
            }).ToList());

            await emailService.SendAsync(new EmailRequest
            {
                ToEmails = newUsersToAssign.Select(u => u.Email).ToList(),
                Subject = "New course assigned to you.",
                Body = EmailUtils.GetAssignmentEmailBody(course.Name)
            });

            await emailService.SendAsync(new EmailRequest
            {
                ToEmails = oldUsersToUnassign.Select(u => u.Email).ToList(),
                Subject = "New course assigned to you.",
                Body = EmailUtils.GetUnassignmentEmailBody(course.Name)
            });
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
        // Add startedAtUtc?
        
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
}