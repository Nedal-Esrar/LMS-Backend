using ErrorOr;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Files;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Questions;
using MLMS.Domain.Sections;
using MLMS.Domain.UserSectionParts;
using File = MLMS.Domain.Files.File;

namespace MLMS.Domain.SectionParts;

public class SectionPartService(
    ISectionPartRepository sectionPartRepository,
    ISectionRepository sectionRepository,
    IFileRepository fileRepository,
    IUserContext userContext) : ISectionPartService
{
    private readonly SectionPartValidator _sectionPartValidator = new();
    
    public async Task<ErrorOr<SectionPart>> CreateAsync(SectionPart sectionPart)
    {
        var validationResult = await _sectionPartValidator.ValidateAsync(sectionPart);

        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }
        
        if (!await sectionRepository.ExistsAsync(sectionPart.SectionId))
        {
            return SectionErrors.NotFound;
        }
        
        var materialValidationError = sectionPart.MaterialType switch
        {
            MaterialType.File => await ValidateForFileTypeAsync(sectionPart),
            MaterialType.Exam => ValidateForExamTypeAsync(sectionPart)
        };

        if (materialValidationError is not null)
        {
            return materialValidationError.Value;
        }

        WipeAdditionalInfo(sectionPart);
        
        sectionPart.Index = await sectionPartRepository.GetMaxIndexBySectionIdAsync(sectionPart.SectionId) + 1;
        
        // If exam, create associations with users that the exam is not done yet.
        
        return await sectionPartRepository.CreateAsync(sectionPart);
    }

    public async Task<ErrorOr<None>> DeleteAsync(long sectionId, long id)
    {
        if (!await sectionPartRepository.ExistsAsync(sectionId, id))
        {
            return SectionPartErrors.NotFound;
        }

        await sectionPartRepository.DeleteAsync(id);

        return None.Value;
    }

    public async Task<ErrorOr<None>> UpdateAsync(long id, SectionPart updatedSectionPart)
    {
        var validationResult = await _sectionPartValidator.ValidateAsync(updatedSectionPart);
        
        if (!validationResult.IsValid)
        {
            return validationResult.ExtractErrors();
        }
        
        var existingSectionPart = await sectionPartRepository.GetByIdAsync(id);

        if (existingSectionPart is null)
        {
            return SectionPartErrors.NotFound;
        }

        if (existingSectionPart.SectionId != updatedSectionPart.SectionId)
        {
            // to be handled soon.
        }

        var materialValidationError = updatedSectionPart.MaterialType switch
        {
            MaterialType.File => await ValidateForFileTypeAsync(updatedSectionPart),
            MaterialType.Exam => ValidateForExamTypeAsync(updatedSectionPart)
        };
        
        if (materialValidationError is not null)
        {
            return materialValidationError.Value;
        }

        WipeAdditionalInfo(updatedSectionPart);
        
        existingSectionPart.MapForUpdate(updatedSectionPart);
        
        await sectionPartRepository.UpdateAsync(existingSectionPart);

        return None.Value;
    }

    public async Task<ErrorOr<SectionPart>> GetByIdAsync(long sectionId, long id)
    {
        if (!await sectionPartRepository.ExistsAsync(sectionId, id))
        {
            return SectionPartErrors.NotFound;
        }

        return await sectionPartRepository.GetByIdAsync(id)!;
    }

    public async Task<ErrorOr<ExamState>> UpdateUserExamStatusAsync(long sectionId, long id, List<(long QuestionId, long ChoiceId)> requestAnswers)
    {
        if (!await sectionPartRepository.ExistsAsync(sectionId, id))
        {
            return SectionPartErrors.NotFound;
        }
        
        var sectionPart = await sectionPartRepository.GetByIdAsync(id)!;
        
        if (sectionPart!.MaterialType != MaterialType.Exam)
        {
            return SectionPartErrors.NotExam;
        }

        requestAnswers = requestAnswers.Distinct().ToList();
        
        if (!CheckQuestionsChoicesAssociations(sectionPart, requestAnswers))
        {
            return SectionPartErrors.QuestionsChoicesAssociationError;
        }

        var answeredQuestions = requestAnswers.Select(x => x.QuestionId)
            .Distinct()
            .Count();
        
        // At this stage, questions are guaranteed to be distinct and in the section part,
        // the staff should have answered all questions.
        if (answeredQuestions != sectionPart.Questions.Count)
        {
            return SectionPartErrors.NotAllQuestionsAnswered;
        }
        
        // Check the number of correctly answered questions and calculate the total points.
        var correctQuestionsAnswers = new List<(Question, Choice)>(); // the question with its correct choice association.

        var totalAnsweredPoints = 0;
        
        foreach (var (questionId, choiceId) in requestAnswers)
        {
            var question = sectionPart.Questions.First(q => q.Id == questionId);
            var choice = question.Choices.First(c => c.Id == choiceId);
            
            var questionCorrectChoice = question.Choices.First(c => c.IsCorrect);

            if (!choice.IsCorrect) // add the question to the correct answers list along with its correct answer.
            {
                correctQuestionsAnswers.Add((question, questionCorrectChoice));
            }
            else // add to the total points to obtain the state.
            {
                totalAnsweredPoints += question.Points;
            }
        }
        
        var examStatus = totalAnsweredPoints < sectionPart.PassThresholdPoints
            ? ExamStatus.Failed
            : ExamStatus.Passed;
        
        // UserExamStates is retrieved for the current user only, so it should be max 1.
        if (sectionPart.UserExamStates.Any())
        {
            sectionPart.UserExamStates.First().Status = examStatus;
        }
        else
        {
            sectionPart.UserExamStates.Add(new UserExamStateForSectionPart
            {
                UserId = userContext.Id!.Value,
                SectionPartId = sectionPart.Id,
                Status = examStatus,
            });
        }

        await sectionPartRepository.UpdateAsync(sectionPart);

        return new ExamState
        {
            CorrectAnswers = correctQuestionsAnswers,
            Status = examStatus
        };
    }

    public async Task<ErrorOr<None>> ToggleUserDoneStatusAsync(long sectionId, long id)
    {
        if (!await sectionPartRepository.ExistsAsync(sectionId, id))
        {
            return SectionPartErrors.NotFound;
        }
        
        await sectionPartRepository.ToggleUserDoneStatusAsync(userContext.Id!.Value, id);

        return None.Value;
    }

    private bool CheckQuestionsChoicesAssociations(SectionPart sectionPart, List<(long QuestionId, long ChoiceId)> requestAnswers)
    {
        var questionChoiceAssociations = sectionPart.Questions
            .SelectMany(question => question.Choices.Select(choice => (question.Id, choice.Id)))
            .ToHashSet();
        
        return requestAnswers.All(a => questionChoiceAssociations.Contains(a));
    }

    private void WipeAdditionalInfo(SectionPart sectionPart)
    {
        switch (sectionPart.MaterialType)
        {
            case MaterialType.Exam:
                sectionPart.FileId = null;
                sectionPart.File = null;
                sectionPart.Link = null;
                break;
            case MaterialType.File:
                sectionPart.Link = null;
                sectionPart.PassThresholdPoints = null;
                sectionPart.Questions = [];
                break;
            case MaterialType.Link:
                sectionPart.FileId = null;
                sectionPart.File = null;
                sectionPart.PassThresholdPoints = null;
                sectionPart.Questions = [];
                break;
        }
    }

    private Error? ValidateForExamTypeAsync(SectionPart sectionPart)
    {
        if (sectionPart.Questions.Any(q => q.Choices.Count == 1))
        {
            return SectionPartErrors.ChoicesCountNotEnough;
        }
        
        var questionsPointsSum = sectionPart.Questions
            .Sum(q => q.Points);

        if (questionsPointsSum < sectionPart.PassThresholdPoints)
        {
            return SectionPartErrors.InvalidThresholdPoints;
        }
        
        var questionWithMultipleCorrectChoices = sectionPart.Questions
            .Any(q => q.Choices.Count(c => c.IsCorrect) > 1);

        if (questionWithMultipleCorrectChoices)
        {
            return SectionPartErrors.QuestionWithMultipleCorrectChoices;
        }
        
        return null;
    }

    private async Task<Error?> ValidateForFileTypeAsync(SectionPart sectionPart)
    {
        if (!await fileRepository.ExistsAsync(sectionPart.FileId!.Value))
        {
            return FileErrors.NotFound;
        }

        return null;
    }
}