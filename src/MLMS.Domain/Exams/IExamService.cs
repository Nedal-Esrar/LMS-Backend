using ErrorOr;
using MLMS.Domain.Common.Models;
using MLMS.Domain.SectionParts;
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
}