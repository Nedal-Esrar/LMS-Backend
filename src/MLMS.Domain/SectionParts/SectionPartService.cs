using ErrorOr;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Exams;
using MLMS.Domain.Files;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Sections;
using MLMS.Domain.UserSectionParts;
using File = MLMS.Domain.Files.File;

namespace MLMS.Domain.SectionParts;

public class SectionPartService(
    ISectionPartRepository sectionPartRepository,
    ISectionRepository sectionRepository,
    IFileRepository fileRepository,
    IUserContext userContext,
    IDbTransactionProvider dbTransactionProvider) : ISectionPartService
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
            MaterialType.Exam => await ValidateForExamTypeAsync(sectionPart.Exam!),
            _ => null
        };

        if (materialValidationError is not null)
        {
            return materialValidationError.Value;
        }

        WipeAdditionalInfo(sectionPart);
        
        sectionPart.Index = await sectionPartRepository.GetMaxIndexBySectionIdAsync(sectionPart.SectionId) + 1;

        if (sectionPart.MaterialType == MaterialType.Exam)
        {
            AssignQuestionAndChoicesIndexesForCreate(sectionPart.Exam!);
        }
    
        var createdSectionPart = default(SectionPart);
        
        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            // TODO: separate repository for exams?
            createdSectionPart = await sectionPartRepository.CreateAsync(sectionPart);
        
            var users = await sectionRepository.GetUsersBySectionIdAsync(sectionPart.SectionId);
            
            await sectionPartRepository.CreateDoneStatesAsync(users.Select(u => new UserSectionPartDone
            {
                UserId = u.Id,
                SectionPartId = sectionPart.Id,
                IsDone = false
            }).ToList());
        
            if (sectionPart.MaterialType == MaterialType.Exam)
            {
                await sectionPartRepository.CreateExamStatesAsync(users.Select(u => new UserExamState
                {
                    UserId = u.Id,
                    ExamId = sectionPart.Exam!.Id,
                    Status = ExamStatus.NotTaken
                }).ToList());
            }
        });

        return createdSectionPart!;
    }

    private void AssignQuestionAndChoicesIndexesForCreate(Exam exam)
    {
        var questionIndex = 0;

        foreach (var question in exam.Questions)
        {
            question.Index = ++questionIndex;

            var choiceIndex = 0;

            foreach (var choice in question.Choices)
            {
                choice.Index = ++choiceIndex;
            }
        }
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
            MaterialType.Exam => await ValidateForExamTypeAsync(updatedSectionPart.Exam!),
            _ => null
        };
        
        if (materialValidationError is not null)
        {
            return materialValidationError.Value;
        }

        WipeAdditionalInfo(updatedSectionPart);

        if (updatedSectionPart.MaterialType == MaterialType.Exam)
        {
            if (existingSectionPart.MaterialType != MaterialType.Exam)
            {
                AssignQuestionAndChoicesIndexesForCreate(updatedSectionPart.Exam!);
            }
            else
            {
                AssignQuestionAndChoicesIndexesForUpdate(existingSectionPart.Exam!, updatedSectionPart.Exam!);
            }
        }

        await dbTransactionProvider.ExecuteInTransactionAsync(async () =>
        {
            // if there were updates to or from Exam type, delete or create associations.
            if (updatedSectionPart.MaterialType == MaterialType.Exam && 
                existingSectionPart.MaterialType != MaterialType.Exam)
            {
                var users = await sectionRepository.GetUsersBySectionIdAsync(updatedSectionPart.SectionId);
            
                await sectionPartRepository.CreateExamStatesAsync(users.Select(u => new UserExamState
                {
                    UserId = u.Id,
                    ExamId = updatedSectionPart.Exam!.Id,
                    Status = ExamStatus.NotTaken
                }).ToList());
            } else if (updatedSectionPart.MaterialType != MaterialType.Exam &&
                       existingSectionPart.MaterialType == MaterialType.Exam)
            {
                await sectionPartRepository.DeleteExamStatesByIdAsync(id);
            }
        
            existingSectionPart.MapForUpdate(updatedSectionPart);
        
            await sectionPartRepository.UpdateAsync(existingSectionPart);
        });

        return None.Value;
    }

    private void AssignQuestionAndChoicesIndexesForUpdate(Exam existingExam, Exam updatedExam)
    {
        var questionIndex = existingExam.Questions
            .Select(q => q.Index)
            .DefaultIfEmpty()
            .Max();

        var oldQuestions = existingExam.Questions
            .ToDictionary(q => q.Id, q => q);

        foreach (var updatedQuestion in updatedExam.Questions)
        {
            if (oldQuestions.TryGetValue(updatedQuestion.Id, out var question))
            {
                var choiceIndex = question.Choices
                    .Select(c => c.Index)
                    .DefaultIfEmpty()
                    .Max();

                var choices = question.Choices
                    .Select(c => c.Id)
                    .ToHashSet();

                foreach (var updatedChoice in updatedQuestion.Choices.Where(updatedChoice => !choices.Contains(updatedChoice.Id)))
                {
                    updatedChoice.Index = ++choiceIndex;
                }
            }
            else
            {
                updatedQuestion.Index = ++questionIndex;

                var choiceIndex = 0;

                foreach (var choice in updatedQuestion.Choices)
                {
                    choice.Index = ++choiceIndex;
                }
            }
        }
    }

    public async Task<ErrorOr<SectionPart>> GetByIdAsync(long sectionId, long id)
    {
        if (!await sectionPartRepository.ExistsAsync(sectionId, id))
        {
            return SectionPartErrors.NotFound;
        }

        return await sectionPartRepository.GetByIdAsync(id)!;
    }

    public async Task<ErrorOr<None>> ToggleUserDoneStatusAsync(long sectionId, long id)
    {
        if (!await sectionPartRepository.ExistsAsync(sectionId, id))
        {
            return SectionPartErrors.NotFound;
        }
        
        await sectionPartRepository.ToggleUserDoneStatusAsync(userContext.Id, id);

        return None.Value;
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
                sectionPart.Exam = null;
                break;
            case MaterialType.Link:
                sectionPart.FileId = null;
                sectionPart.File = null;
                sectionPart.Exam = null;
                break;
        }
    }

    private async Task<Error?> ValidateForExamTypeAsync(Exam exam)
    {
        if (exam.Questions.Any(q => q.Choices.Count == 1))
        {
            return SectionPartErrors.ChoicesCountNotEnough;
        }
        
        var questionsPointsSum = exam.Questions
            .Sum(q => q.Points);

        if (questionsPointsSum < exam.PassThresholdPoints)
        {
            return SectionPartErrors.InvalidThresholdPoints;
        }
        
        var questionWithMultipleCorrectChoices = exam.Questions
            .Any(q => q.Choices.Count(c => c.IsCorrect) > 1);

        if (questionWithMultipleCorrectChoices)
        {
            return SectionPartErrors.QuestionWithMultipleCorrectChoices;
        }
        
        // validate images attached to files.
        foreach (var question in exam.Questions)
        {
            if (!question.ImageId.HasValue)
            {
                continue;
            }
            
            var imageFile = await fileRepository.GetByIdAsync(question.ImageId.Value);

            if (imageFile is null)
            {
                return FileErrors.NotFound;
            }

            if (!imageFile.IsImage())
            {
                return FileErrors.NotImage;
            }
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