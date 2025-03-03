using ErrorOr;
using MLMS.Domain.Common;
using MLMS.Domain.Common.Exceptions;
using MLMS.Domain.Common.Interfaces;
using MLMS.Domain.Common.Models;
using MLMS.Domain.Exams;
using MLMS.Domain.Files;
using MLMS.Domain.Identity.Interfaces;
using MLMS.Domain.Sections;
using MLMS.Domain.Users;
using MLMS.Domain.UsersCourses;
using MLMS.Domain.UserSectionParts;

namespace MLMS.Domain.SectionParts;

public class SectionPartService(
    ISectionPartRepository sectionPartRepository,
    ISectionService sectionService,
    IUserService userService,
    IFileService fileService,
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

        var sectionExistenceResult = await sectionService.CheckExistenceAsync(sectionPart.SectionId);
        
        if (sectionExistenceResult.IsError)
        {
            return sectionExistenceResult.Errors;
        }
        
        var materialValidationResult = sectionPart.MaterialType switch
        {
            MaterialType.File => await ValidateForFileTypeAsync(sectionPart),
            MaterialType.Exam => await ValidateForExamTypeAsync(sectionPart.Exam!),
            _ => None.Value
        };

        if (materialValidationResult.IsError)
        {
            return materialValidationResult.Errors;
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
        
            var usersRetrievalResult = await userService.GetBySectionIdAsync(sectionPart.SectionId);

            if (usersRetrievalResult.IsError)
            {
                throw new UnoccasionalErrorException(usersRetrievalResult.Errors);
            }

            var users = usersRetrievalResult.Value;
            
            await sectionPartRepository.CreateDoneStatesAsync(users.Select(u => new UserSectionPart
            {
                UserId = u.Id,
                SectionPartId = sectionPart.Id,
                Status = SectionPartStatus.NotViewed
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
        
        var sectionExistenceResult = await sectionService.CheckExistenceAsync(updatedSectionPart.SectionId);
        
        if (sectionExistenceResult.IsError)
        {
            return sectionExistenceResult.Errors;
        }

        if (existingSectionPart.SectionId != updatedSectionPart.SectionId)
        {
            // to be handled soon.
        }

        var materialValidationResult = updatedSectionPart.MaterialType switch
        {
            MaterialType.File => await ValidateForFileTypeAsync(updatedSectionPart),
            MaterialType.Exam => await ValidateForExamTypeAsync(updatedSectionPart.Exam!),
            _ => None.Value
        };
        
        if (materialValidationResult.IsError)
        {
            return materialValidationResult.Errors;
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
                var usersRetrievalResult = await userService.GetBySectionIdAsync(updatedSectionPart.SectionId);

                if (usersRetrievalResult.IsError)
                {
                    throw new UnoccasionalErrorException(usersRetrievalResult.Errors);
                }

                var users = usersRetrievalResult.Value;
            
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

    public async Task<ErrorOr<None>> ChangeUserSectionPartStatusAsync(long sectionId, long id,
        SectionPartStatus status)
    {
        if (!await sectionPartRepository.ExistsAsync(sectionId, id))
        {
            return SectionPartErrors.NotFound;
        }

        var userStatus = await sectionPartRepository.GetStatusByIdAndUserAsync(id, userContext.Id);

        if (userStatus.Status == status)
        {
            return SectionPartErrors.InvalidSectionPartStatusTransition;
        }

        switch (userStatus.Status)
        {
            case SectionPartStatus.NotViewed:
                if (status == SectionPartStatus.Done)
                {
                    return SectionPartErrors.InvalidSectionPartStatusTransition;
                }
                break;
            case SectionPartStatus.Viewed:
                if (status == SectionPartStatus.NotViewed)
                {
                    return SectionPartErrors.InvalidSectionPartStatusTransition;
                }
                break;
            case SectionPartStatus.Done:
                if (status == SectionPartStatus.NotViewed)
                {
                    return SectionPartErrors.InvalidSectionPartStatusTransition;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        await sectionPartRepository.SetUserStatusAsync(userContext.Id, id, status);

        return None.Value;
    }

    public async Task<ErrorOr<None>> ResetDoneStatesByUserCoursesAsync(List<UserCourse> userCourses)
    {
        await sectionPartRepository.ResetDoneStatesByUserCoursesAsync(userCourses);

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

    private async Task<ErrorOr<None>> ValidateForExamTypeAsync(Exam exam)
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
            
            var imageRetrievalResult = await fileService.GetByIdAsync(question.ImageId.Value);

            if (imageRetrievalResult.IsError)
            {
                return imageRetrievalResult.Errors;
            }

            if (!imageRetrievalResult.Value.IsImage())
            {
                return FileErrors.NotImage;
            }
        }
        
        return None.Value;
    }

    private async Task<ErrorOr<None>> ValidateForFileTypeAsync(SectionPart sectionPart)
    {
        return await fileService.CheckExistenceAsync(sectionPart.FileId!.Value);
    }
}