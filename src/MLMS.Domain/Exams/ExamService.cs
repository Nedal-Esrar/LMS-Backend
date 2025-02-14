using ErrorOr;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
using MLMS.Domain.ExamSessions;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.SectionParts;
using MLMS.Domain.UserSectionParts;

namespace MLMS.Domain.Exams;

public class ExamService(
    IExamRepository examRepository,
    IUserContext userContext,
    IDbTransactionProvider dbTransactionProvider,
    ISectionPartRepository sectionPartRepository) : IExamService
{
    public async Task<ErrorOr<None>> StartSessionAsync(long examId)
    {
        if (!await examRepository.ExistsAsync(examId))
        {
            return ExamErrors.NotFound;
        }
        
        var userId = userContext.Id;
        
        if (await examRepository.IsSessionStartedAsync(userId, examId))
        {
            return ExamErrors.SessionAlreadyStarted;
        }

        var newExamSession = new ExamSession
        {
            ExamId = examId,
            UserId = userId,
            StartDateUtc = DateTime.UtcNow
        };
        
        await examRepository.CreateSessionAsync(newExamSession);

        return None.Value;
    }

    public async Task<ErrorOr<bool>> IsSessionStartedAsync(long examId)
    {
        if (!await examRepository.ExistsAsync(examId))
        {
            return ExamErrors.NotFound;
        }
        
        var userId = userContext.Id;
        
        if (await examRepository.IsSessionDueAsync(userId, examId))
        {
            await FinishCurrentSessionAsync(examId);
        }

        return await examRepository.IsSessionStartedAsync(userId, examId);
    }

    public async Task<ErrorOr<ExamSessionState>> GetSessionDetailsAsync(long examId)
    {
        if (!await examRepository.ExistsAsync(examId))
        {
            return ExamErrors.NotFound;
        }
        
        var userId = userContext.Id;

        if (await examRepository.IsSessionFinishedAsync(userId, examId))
        {
            return ExamErrors.SessionEnded;
        }
        
        var examSession = await examRepository.GetCurrentSessionAsync(userId, examId);
        
        var examQuestion = await examRepository.GetExamQuestionIdsAsync(examId);
        
        var examSessionQuestionChoices = (await examRepository.GetQuestionChoicesAsync(examSession.Id))
            .ToDictionary(x => x.QuestionId, x => x.ChoiceId);

        return new ExamSessionState
        {
            CheckpointQuestionId = examSession.CheckpointQuestionId,
            Questions = examQuestion.Select(q => (q.Id,
                examSessionQuestionChoices.TryGetValue(q.Id, out var answer) && answer.HasValue, q.Index)).ToList(),
            StartDateUtc = examSession.StartDateUtc,
            DurationMinutes = examSession.Exam.DurationMinutes
        };
    }

    public async Task<ErrorOr<ExamSessionQuestionChoice>> GetQuestionAsync(long examId, long questionId)
    {
        if (!await examRepository.ExistsAsync(examId))
        {
            return ExamErrors.NotFound;
        }
        
        var userId = userContext.Id;
        
        if (await examRepository.IsSessionFinishedAsync(userId, examId))
        {
            return ExamErrors.SessionEnded;
        }
        
        if (!await examRepository.HasQuestionAsync(examId, questionId))
        {
            return ExamErrors.QuestionNotFound;
        }
        
        var examSession = await examRepository.GetCurrentSessionAsync(userId, examId);
        
        if (!await examRepository.SessionStateExistsAsync(examSession.Id, questionId))
        {
            await examRepository.UpsertSessionQuestionChoice(new ExamSessionQuestionChoice
            {
                ExamSessionId = examSession.Id,
                QuestionId = questionId,
                ChoiceId = null // not answered.
            });
        }
        
        return await examRepository.GetSessionQuestionChoiceAsync(examSession.Id, questionId)!;
    }

    public async Task<ErrorOr<None>> AnswerQuestionAsync(long examId, long questionId, long choiceId)
    {
        if (!await examRepository.ExistsAsync(examId))
        {
            return ExamErrors.NotFound;
        }
        
        var userId = userContext.Id;
        
        if (await examRepository.IsSessionFinishedAsync(userId, examId))
        {
            return ExamErrors.SessionEnded;
        }
        
        if (!await examRepository.HasQuestionAsync(examId, questionId))
        {
            return ExamErrors.QuestionNotFound;
        }
        
        if (!await examRepository.DoesQuestionHasChoiceAsync(questionId, choiceId))
        {
            return ExamErrors.ChoiceNotFound;
        }
        
        var examSession = await examRepository.GetCurrentSessionAsync(userId, examId);
        
        await examRepository.UpsertSessionQuestionChoice(new ExamSessionQuestionChoice
        {
            ExamSessionId = examSession.Id,
            QuestionId = questionId,
            ChoiceId = choiceId
        });

        return None.Value;
    }

    public async Task<ErrorOr<ExamStatus>> FinishCurrentSessionAsync(long examId)
    {
        if (!await examRepository.ExistsAsync(examId))
        {
            return ExamErrors.NotFound;
        }
        
        var userId = userContext.Id;
        
        var examSession = await examRepository.GetCurrentSessionAsync(userId, examId);
        
        var exam = await examRepository.GetByIdAsync(examId);
        
        var examSessionQuestionChoices = (await examRepository.GetQuestionChoicesAsync(examSession.Id))
            .ToDictionary(x => x.QuestionId, x => x.ChoiceId);
        
        var totalAnsweredPoints = 0;

        foreach (var question in exam!.Questions)
        {
            if (!examSessionQuestionChoices.TryGetValue(question.Id, out var choiceId) || choiceId is null)
            {
                continue;
            }
            
            if (question.Choices.First(c => c.Id == choiceId).IsCorrect)
            {
                totalAnsweredPoints += question.Points;
            }
        }
        
        var examStatus = totalAnsweredPoints < exam.PassThresholdPoints
            ? ExamStatus.Failed
            : ExamStatus.Passed;

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            var userExamState = await examRepository.GetUserExamStateAsync(userId, examId)
                ?? new UserExamState
                {
                    UserId = userId,
                    ExamId = examId
                };

            userExamState.Status = examStatus;

            examSession.IsDone = true;
            examSession.Grade = totalAnsweredPoints;

            await examRepository.UpdateExamStateAsync(userExamState);
            
            await examRepository.UpdateSessionAsync(examSession);

            if (examStatus == ExamStatus.Passed)
            {
                var sectionPart = await examRepository.GetSectionPartByExamIdAsync(examId);
                
                await sectionPartRepository.SetUserStatusAsync(userId, sectionPart!.Id,
                    SectionPartStatus.Done);
            }
        });

        return examStatus;
    }

    public async Task<ErrorOr<Exam>> GetByIdAsync(long id)
    {
        var exam = await examRepository.GetByIdAsync(id);

        if (exam is null)
        {
            return ExamErrors.NotFound;
        }
        
        exam.MaxGradePoints = exam.Questions.Sum(q => q.Points);

        return exam;
    }
}