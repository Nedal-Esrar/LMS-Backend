using MLMS.API.Courses.Requests;
using MLMS.API.Courses.Responses;
using MLMS.API.Departments;
using MLMS.API.SectionParts;
using MLMS.Domain.CourseAssignments;
using MLMS.Domain.Courses;
using MLMS.Domain.Sections;
using MLMS.Domain.UsersCourses;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.Courses;

[Mapper]
public static partial class CourseMapper
{
    public static partial Course ToDomain(this InitializeCourseRequest request);
    
    public static partial Course ToDomain(this UpdateCourseRequest request);

    public static CourseSimplifiedResponse ToSimplifiedContract(this Course course)
    {
        var response = course.ToSimplifiedContractInternal();
        
        response.Status = course.UsersCourses.FirstOrDefault()?.Status;
        response.CreatedByName = course.CreatedBy is null
            ? string.Empty
            : $"{course.CreatedBy.FirstName} {course.CreatedBy.MiddleName} {course.CreatedBy.LastName}";

        return response;
    }
    
    private static partial CourseSimplifiedResponse ToSimplifiedContractInternal(this Course course);

    public static CourseDetailedResponse ToDetailedContract(this Course course)
    {
        var response = course.ToDetailedContractInternal();
        
        response.Status = course.UsersCourses.FirstOrDefault()?.Status;
        response.FinishedAtUtc = course.UsersCourses.FirstOrDefault()?.FinishedAtUtc;
        response.CreatedByName = course.CreatedBy is null
            ? string.Empty
            : $"{course.CreatedBy.FirstName} {course.CreatedBy.MiddleName} {course.CreatedBy.LastName}";

        return response;
    }
    
    private static SectionDetailedResponse ToContract(this Section section)
    {
        var response = section.ToContractInternal();
        
        response.SectionParts = section.SectionParts.Select(p => p.ToContract()).ToList();
        
        return response;
    }
    
    private static partial SectionDetailedResponse ToContractInternal(this Section section);
    
    private static partial CourseDetailedResponse ToDetailedContractInternal(this Course course);

    public static AssignmentResponse ToContract(this CourseAssignment assignment)
    {
        return new AssignmentResponse
        {
            MajorId = assignment.MajorId,
            MajorName = assignment.Major.Name,
            Department = assignment.Major.Department.ToContract()
        };
    }
    
    public static CourseParticipantResponse ToParticipantContract(this UserCourse userCourse)
    {
        return new CourseParticipantResponse
        {
            UserId = userCourse.UserId,
            UserName = $"{userCourse.User.FirstName} {userCourse.User.MiddleName} {userCourse.User.LastName}",
            Status = userCourse.Status,
            FinishedAtUtc = userCourse.FinishedAtUtc,
            StartedAtUtc = userCourse.StartedAtUtc
        };
    }
}