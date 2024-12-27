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
    
        var createdSectionPart = new SectionPart();
        
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

        return createdSectionPart;
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

    public async Task<ErrorOr<SectionPart>> GetByIdAsync(long sectionId, long id)
    {
        if (!await sectionPartRepository.ExistsAsync(sectionId, id))
        {
            return SectionPartErrors.NotFound;
        }

        return await sectionPartRepository.GetByIdAsync(id)!;
    }

    // This should be re-evaluated.
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
        if (answeredQuestions != sectionPart.Exam!.Questions.Count)
        {
            return SectionPartErrors.NotAllQuestionsAnswered;
        }
        
        // Check the number of correctly answered questions and calculate the total points.
        var correctQuestionsAnswers = new List<(Question, Choice)>(); // the question with its correct choice association.

        var totalAnsweredPoints = 0;
        
        foreach (var (questionId, choiceId) in requestAnswers)
        {
            var question = new Question();
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
        
        var examStatus = totalAnsweredPoints < sectionPart.Exam.PassThresholdPoints
            ? ExamStatus.Failed
            : ExamStatus.Passed;
        
        // UserExamStates is retrieved for the current user only, so it should be max 1.
        if (sectionPart.UserExamStates.Any())
        {
            sectionPart.UserExamStates.First().Status = examStatus;
        }
        else
        {
            sectionPart.UserExamStates.Add(new UserExamState
            {
                UserId = userContext.Id!.Value,
                ExamId = sectionPart.ExamId!.Value,
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

    // Also this.
    private bool CheckQuestionsChoicesAssociations(SectionPart sectionPart, List<(long QuestionId, long ChoiceId)> requestAnswers)
    {
        var questionChoiceAssociations = sectionPart.Exam!.Questions
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