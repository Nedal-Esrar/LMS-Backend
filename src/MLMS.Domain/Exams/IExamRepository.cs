using ErrorOr;
using MLMS.Domain.ExamSessions;
using MLMS.Domain.UsersCourses;
using MLMS.Domain.UserSectionParts;

namespace MLMS.Domain.Exams;

public interface IExamRepository
{
    Task<bool> IsSessionStartedAsync(int userId, long examId);
    
    Task<bool> IsSessionDueAsync(int userId, long examId);
    
    Task<bool> ExistsAsync(long examId);
    
    Task CreateSessionAsync(ExamSession examSession);
    
    Task<ExamSession> GetCurrentSessionAsync(int userId, long examId);
    
    Task<List<long>> GetExamQuestionIdsAsync(long examId);
    
    Task<List<ExamSessionQuestionChoice>> GetQuestionChoicesAsync(Guid examSessionId);
    
    Task<bool> HasQuestionAsync(long examId, long questionId);
    
    Task<ExamSessionQuestionChoice?> GetSessionQuestionChoiceAsync(Guid examSessionId, long questionId);
    
    Task UpsertSessionQuestionChoice(ExamSessionQuestionChoice userSessionQuestionChoice);
    
    Task<bool> DoesQuestionHasChoiceAsync(long questionId, long choiceId);
    
    Task<bool> SessionStateExistsAsync(Guid examSessionId, long questionId);
    
    Task<Exam?> GetByIdAsync(long examId);
    
    Task<UserExamState?> GetUserExamStateAsync(int userId, long examId);
    
    Task UpdateExamStateAsync(UserExamState userExamState);
    
    Task UpdateSessionAsync(ExamSession examSession);
    
    Task ResetExamStatesByUserCoursesAsync(List<UserCourse> expiredUserCourses);
}