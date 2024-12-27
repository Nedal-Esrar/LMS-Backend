using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Exams;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.SectionParts;
using MLMS.Domain.UserSectionParts;
using MLMS.Infrastructure.Common;

namespace MLMS.Infrastructure.SectionParts;

public class SectionPartRepository(
    LmsDbContext context,
    IUserContext userContext) : ISectionPartRepository
{
    public async Task<SectionPart> CreateAsync(SectionPart sectionPart)
    {
        await context.SectionParts.AddAsync(sectionPart);
        
        await context.SaveChangesAsync();
        
        return sectionPart;
    }

    public async Task<int> GetMaxIndexBySectionIdAsync(long sectionId)
    {
        return await context.SectionParts
            .Where(x => x.SectionId == sectionId)
            .Select(x => x.Index)
            .DefaultIfEmpty()
            .MaxAsync();
    }

    public async Task<bool> ExistsAsync(long sectionId, long id)
    {
        return await context.SectionParts.AnyAsync(x => x.SectionId == sectionId && x.Id == id);
    }

    public async Task DeleteAsync(long id)
    {
        var sectionPart = await context.SectionParts.FindAsync(id);

        if (sectionPart is null)
        {
            return;
        }
        
        context.SectionParts.Remove(sectionPart);
        
        await context.SaveChangesAsync();
    }

    public async Task<SectionPart?> GetByIdAsync(long id)
    {
        var userId = userContext.Id!.Value;
        
        var query = context.SectionParts.Where(sp => sp.Id == id)
            .Include(x => x.File)
            .Include(x => x.Exam)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Image)
            .Include(x => x.Exam)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Choices)
            .Include(x => x.UserExamStates.Where(ue => ue.UserId == id))
            .Include(x => x.UserSectionPartStatuses.Where(usp => usp.UserId == userId))
            .AsSplitQuery()
            .AsNoTracking();
        
        return await query.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(SectionPart sectionPart)
    {
        var existingSectionPart = await context.SectionParts.Where(sp => sp.Id == sectionPart.Id)
            .Include(x => x.Exam)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Choices)
            .AsSplitQuery()
            .FirstOrDefaultAsync();
        
        if (existingSectionPart is null)
        {
            return;
        }
        
        // map basic values
        existingSectionPart.Title = sectionPart.Title;
        existingSectionPart.MaterialType = sectionPart.MaterialType;
        existingSectionPart.Link = sectionPart.Link;
        existingSectionPart.FileId = sectionPart.FileId;
        
        // if the updated type is exam, map questions and choices.
        if (sectionPart.MaterialType == MaterialType.Exam)
        {
            MapExam(existingSectionPart, sectionPart);
        }
        
        await context.SaveChangesAsync();
    }

    private void MapExam(SectionPart existingSectionPart, SectionPart sectionPart)
    {
        // basic exam mapping
        existingSectionPart.Exam!.DurationMinutes = sectionPart.Exam!.DurationMinutes;
        existingSectionPart.Exam!.PassThresholdPoints = sectionPart.Exam!.PassThresholdPoints;
        
        var existingQuestions = existingSectionPart.Exam.Questions;
        var newQuestions = sectionPart.Exam.Questions;
        
        var existingQuestionsMap = existingSectionPart.Exam.Questions
            .ToDictionary(x => x.Id, x => x);
        
        var newQuestionsIds = sectionPart.Exam.Questions
            .Select(q => q.Id)
            .ToHashSet();
        
        // remove questions that are not in the new list
        foreach (var existingQuestion in existingQuestions.Where(q => !newQuestionsIds.Contains(q.Id)))
        {
            context.Questions.Remove(existingQuestion);
        }
        
        // add new questions
        foreach (var newQuestion in newQuestions)
        {
            if (existingQuestionsMap.TryGetValue(newQuestion.Id, out var existingQuestion)) // to update.
            {
                // question basic update
                MapQuestion(existingQuestion, newQuestion);
            }
            else // to create.
            {
                context.Questions.Add(newQuestion);
            }
        }
        
        context.Exams.Update(existingSectionPart.Exam);
    }

    private void MapQuestion(Question existingQuestion, Question newQuestion)
    {
        // questions basic mapping
        existingQuestion.Text = newQuestion.Text;
        existingQuestion.Points = newQuestion.Points;
        existingQuestion.ImageId = newQuestion.ImageId;
        
        // map choices as question are mapped for exam
        var existingChoices = existingQuestion.Choices;
        var newChoices = newQuestion.Choices;

        var existingChoicesMap = existingQuestion.Choices
            .ToDictionary(x => x.Id, x => x);
        
        var newChoicesIds = newQuestion.Choices
            .Select(q => q.Id)
            .ToHashSet();
        
        // remove questions that are not in the new list
        foreach (var existingChoice in existingChoices.Where(q => !newChoicesIds.Contains(q.Id)))
        {
            context.Choices.Remove(existingChoice);
        }
        
        // add new questions
        foreach (var newChoice in newChoices)
        {
            if (existingChoicesMap.TryGetValue(newChoice.Id, out var existingChoice)) // to update.
            {
                // choice basic mapping
                existingChoice.Text = newChoice.Text;
                existingChoice.IsCorrect = newChoice.IsCorrect;
            }
            else // to create.
            {
                context.Choices.Add(newChoice);
            }
        }

        context.Questions.Update(existingQuestion);
    }

    public async Task ToggleUserDoneStatusAsync(int userId, long id)
    {
        await context.UserSectionPartDoneRelations
            .Where(x => x.UserId == userId && x.SectionPartId == id)
            .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsDone, x => !x.IsDone));
    }

    public async Task<List<UserExamState>> GetExamStatusesByCourseAndUserAsync(long id, int userId)
    {
        var query = from c in context.Courses
            join s in context.Sections on c.Id equals s.CourseId
            join sp in context.SectionParts on s.Id equals sp.SectionId
            join e in context.Exams on sp.ExamId equals e.Id
            join usp in context.UserExamStateRelations on e.Id equals usp.ExamId
            where c.Id == id && usp.UserId == userId
            select usp;
        
        return await query.AsNoTracking()
            .ToListAsync();
    }

    public async Task CreateDoneStatesAsync(List<UserSectionPartDone> doneStates)
    {
        context.UserSectionPartDoneRelations.AddRange(doneStates);
        
        await context.SaveChangesAsync();
    }

    public async Task CreateExamStatesAsync(List<UserExamState> examStates)
    {
        context.UserExamStateRelations.AddRange(examStates);
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteExamStatesByIdAsync(long id)
    {
        var query = from ue in context.UserExamStateRelations
            join e in context.Exams on ue.ExamId equals e.Id
            join sp in context.SectionParts on e.SectionPartId equals sp.Id
            where sp.Id == id
            select ue;

        await query.ExecuteDeleteAsync();
    }
}