using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Exams;
using MLMS.Domain.ExamSessions;
using MLMS.Domain.UsersCourses;
using MLMS.Domain.UserSectionParts;
using MLMS.Infrastructure.Common;

namespace MLMS.Infrastructure.Exams;

public class ExamRepository(LmsDbContext context) : IExamRepository
{
    public async Task<bool> IsSessionStartedAsync(int userId, long examId)
    {
        var session = await context.ExamSessions
            .Include(x => x.Exam)
            .Where(es => es.UserId == userId && es.ExamId == examId)
            .OrderByDescending(es => es.StartDateUtc)
            .FirstOrDefaultAsync();

        if (session is null)
        {
            return false;
        }

        return !session.IsDone && 
               (DateTime.UtcNow - session.StartDateUtc).Minutes < session.Exam.DurationMinutes;
    }

    public async Task<bool> IsSessionDueAsync(int userId, long examId)
    {
        var session = await context.ExamSessions
            .Include(x => x.Exam)
            .Where(es => es.UserId == userId && es.ExamId == examId)
            .OrderByDescending(es => es.StartDateUtc)
            .FirstOrDefaultAsync();

        if (session is null)
        {
            return false;
        }

        return !session.IsDone && 
               (DateTime.UtcNow - session.StartDateUtc).Minutes >= session.Exam.DurationMinutes;
    }

    public async Task<bool> ExistsAsync(long examId)
    {
        return await context.Exams.AnyAsync(e => e.Id == examId);
    }

    public async Task CreateSessionAsync(ExamSession examSession)
    {
        context.ExamSessions.Add(examSession);
        
        await context.SaveChangesAsync();
    }

    public async Task<ExamSession> GetCurrentSessionAsync(int userId, long examId)
    {
        return await context.ExamSessions
            .Include(es => es.Exam)
            .Where(es => es.UserId == userId && es.ExamId == examId)
            .OrderByDescending(es => es.StartDateUtc)
            .FirstOrDefaultAsync()!;
    }

    public async Task<List<Question>> GetExamQuestionIdsAsync(long examId)
    {
        return await context.Questions
            .Where(q => q.ExamId == examId)
            .ToListAsync();
    }

    public async Task<List<ExamSessionQuestionChoice>> GetQuestionChoicesAsync(Guid examSessionId)
    {
        return await context.ExamSessionQuestionChoices
            .Where(es => es.ExamSessionId == examSessionId)
            .ToListAsync();
    }

    public async Task<bool> HasQuestionAsync(long examId, long questionId)
    {
        return await context.Questions.AnyAsync(q => q.Id == questionId && q.ExamId == examId);
    }

    public async Task<ExamSessionQuestionChoice?> GetSessionQuestionChoiceAsync(Guid examSessionId, long questionId)
    {
        return await context.ExamSessionQuestionChoices
            .Include(esqc => esqc.Question)
            .ThenInclude(q => q.Choices)
            .Include(esqc => esqc.Question)
            .ThenInclude(q => q.Image)
            .FirstOrDefaultAsync(es => es.ExamSessionId == examSessionId && es.QuestionId == questionId);
    }

    public async Task UpsertSessionQuestionChoice(ExamSessionQuestionChoice userSessionQuestionChoice)
    {
        if (!await context.ExamSessionQuestionChoices.AnyAsync(es =>
            es.ExamSessionId == userSessionQuestionChoice.ExamSessionId &&
            es.QuestionId == userSessionQuestionChoice.QuestionId))
        {
            context.ExamSessionQuestionChoices.Add(userSessionQuestionChoice);
        }
        else
        {
            context.ExamSessionQuestionChoices.Update(userSessionQuestionChoice);
        }

        await context.SaveChangesAsync();
    }

    public async Task<bool> DoesQuestionHasChoiceAsync(long questionId, long choiceId)
    {
        return await context.Choices.AnyAsync(c => c.QuestionId == questionId && c.Id == choiceId);
    }

    public async Task<bool> SessionStateExistsAsync(Guid examSessionId, long questionId)
    {
        return await context.ExamSessionQuestionChoices.AnyAsync(es =>
            es.ExamSessionId == examSessionId && es.QuestionId == questionId);
    }

    public async Task<Exam?> GetByIdAsync(long examId)
    {
        return await context.Exams
            .Include(e => e.Questions)
            .ThenInclude(q => q.Choices)
            .FirstOrDefaultAsync(e => e.Id == examId);
    }

    public async Task<UserExamState?> GetUserExamStateAsync(int userId, long examId)
    {
        return await context.UserExamStateRelations
            .FirstOrDefaultAsync(ues => ues.UserId == userId && ues.ExamId == examId);
    }

    public async Task UpdateExamStateAsync(UserExamState userExamState)
    {
        if (!await context.UserExamStateRelations.AnyAsync(ues =>
            ues.UserId == userExamState.UserId && ues.ExamId == userExamState.ExamId))
        {
            context.UserExamStateRelations.Add(userExamState);
        }
        else
        {
            context.UserExamStateRelations.Update(userExamState);
        }

        await context.SaveChangesAsync();
    }

    public async Task UpdateSessionAsync(ExamSession examSession)
    {
        context.ExamSessions.Update(examSession);
        
        await context.SaveChangesAsync();
    }

    public async Task ResetExamStatesByUserCoursesAsync(List<UserCourse> expiredUserCourses)
    {
        var query = from uc in expiredUserCourses.AsQueryable()
            join s in context.Sections on uc.CourseId equals s.CourseId
            join sp in context.SectionParts on s.Id equals sp.SectionId
            join e in context.Exams on sp.ExamId equals e.Id
            join ues in context.UserExamStateRelations on e.Id equals ues.ExamId
            where ues.UserId == uc.UserId
            select ues;

        await query.ExecuteUpdateAsync(u => u.SetProperty(x => x.Status, ExamStatus.NotTaken));
    }

    public async Task<bool> IsSessionFinishedAsync(int userId, long examId)
    {
        var session = await context.ExamSessions
            .Include(x => x.Exam)
            .Where(es => es.UserId == userId && es.ExamId == examId)
            .OrderByDescending(es => es.StartDateUtc)
            .FirstOrDefaultAsync();

        if (session is null)
        {
            return true;
        }

        return session.IsDone ||
               (DateTime.UtcNow - session.StartDateUtc).Minutes >= session.Exam.DurationMinutes;
    }
}