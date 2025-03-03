using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.SectionParts;
using MLMS.Domain.UsersCourses;
using MLMS.Domain.UserSectionParts;

namespace MLMS.Domain.Exams;

public interface IExamService
{
    Task<ErrorOr<None>> StartSessionAsync(long examId);
    
    Task<ErrorOr<bool>> IsSessionStartedAsync(long examId);
    
    Task<ErrorOr<ExamSessionState>> GetSessionDetailsAsync(long examId);
    
    Task<ErrorOr<ExamSessionQuestionChoice>> GetQuestionAsync(long examId, long questionId);
    
    Task<ErrorOr<None>> AnswerQuestionAsync(long examId, long questionId, long choiceId);
    
    Task<ErrorOr<ExamStatus>> FinishCurrentSessionAsync(long examId);
    
    Task<ErrorOr<Exam>> GetByIdAsync(long id);
    
    Task<ErrorOr<bool>> IsSessionDueAsync(int userId, long examId);
    
    Task<ErrorOr<List<UserExamState>>> GetExamStatusesByCourseAndUserAsync(long id, int userId);
    
    Task<ErrorOr<None>> ResetExamStatesByUserCoursesAsync(List<UserCourse> userCourses);
}