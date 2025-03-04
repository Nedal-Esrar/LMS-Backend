using Microsoft.EntityFrameworkCore;
using MLMS.Domain.Exams;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.SectionParts;
using MLMS.Domain.UsersCourses;
using MLMS.Domain.UserSectionParts;
using MLMS.Infrastructure.Persistence;

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
        var userId = userContext.Id;
        
        var query = context.SectionParts.Where(sp => sp.Id == id)
            .Include(x => x.File)
            .Include(x => x.Exam)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Image)
            .Include(x => x.Exam)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Choices)
            .Include(x => x.Exam)
                .ThenInclude(x => x.UserExamStates.Where(ue => ue.UserId == id))
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
        if (existingSectionPart.MaterialType == MaterialType.Exam 
            && sectionPart.MaterialType == MaterialType.Exam)
        {
            MapExam(existingSectionPart, sectionPart);
        }
        else if (existingSectionPart.MaterialType == MaterialType.Exam 
                 && sectionPart.MaterialType != MaterialType.Exam)
        {
            context.Exams.Remove(existingSectionPart.Exam!);
        }
        else if (existingSectionPart.MaterialType != MaterialType.Exam 
                 && sectionPart.MaterialType == MaterialType.Exam)
        {
            existingSectionPart.Exam = sectionPart.Exam;
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
                newQuestion.ExamId = sectionPart.ExamId!.Value;
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

                context.Choices.Update(existingChoice);
            }
            else // to create.
            {
                newChoice.QuestionId = existingQuestion.Id;
                context.Choices.Add(newChoice);
            }
        }

        context.Questions.Update(existingQuestion);
    }

    public async Task SetUserStatusAsync(int userId, long id, SectionPartStatus status)
    {
        if (!await context.UserSectionPartRelations.AnyAsync(d => d.SectionPartId == id && d.UserId == userId))
        {
            context.UserSectionPartRelations.Add(new UserSectionPart
            {
                SectionPartId = id,
                UserId = userId,
                Status = status
            });

            await context.SaveChangesAsync();

            return;
        }
        
        await context.UserSectionPartRelations
            .Where(x => x.UserId == userId && x.SectionPartId == id)
            .ExecuteUpdateAsync(u => u.SetProperty(x => x.Status, status));
    }

    public async Task CreateDoneStatesAsync(List<UserSectionPart> doneStates)
    {
        context.UserSectionPartRelations.AddRange(doneStates);
        
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
            join sp in context.SectionParts on e.Id equals sp.ExamId
            where sp.Id == id
            select ue;

        await query.ExecuteDeleteAsync();
    }

    public async Task ResetDoneStatesByUserCoursesAsync(List<UserCourse> expiredUserCourses)
    {
        var query = from uc in expiredUserCourses.AsQueryable()
            join s in context.Sections on uc.CourseId equals s.CourseId
            join sp in context.SectionParts on s.Id equals sp.SectionId
            join usp in context.UserSectionPartRelations on sp.Id equals usp.SectionPartId
            where usp.UserId == uc.UserId
            select usp;

        await query.ExecuteUpdateAsync(u => u.SetProperty(x => x.Status, SectionPartStatus.NotViewed));
    }

    public async Task<UserSectionPart> GetStatusByIdAndUserAsync(long id, int userId)
    {
        var userSectionPart = await context.UserSectionPartRelations
            .Where(d => d.SectionPartId == id && d.UserId == userId)
            .FirstOrDefaultAsync();
        if (userSectionPart is null)
        {
            userSectionPart = new UserSectionPart
            {
                SectionPartId = id,
                UserId = userId,
                Status = SectionPartStatus.NotViewed
            };
            
            context.UserSectionPartRelations.Add(userSectionPart);

            await context.SaveChangesAsync();
        }

        return userSectionPart;
    }

    public async Task UpdateStatusAsync(UserSectionPart userStatus)
    {
        context.UserSectionPartRelations.Update(userStatus);
        
        await context.SaveChangesAsync();
    }
}