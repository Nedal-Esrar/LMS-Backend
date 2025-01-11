using MLMS.API.Exams.Requests;
using MLMS.API.Exams.Responses;
using MLMS.API.Files;
using MLMS.Domain.Exams;
using Riok.Mapperly.Abstractions;

namespace MLMS.API.Exams;

[Mapper]
public static partial class ExamsMapper
{
    private static partial Choice ToDomain(this ChoiceCreateRequest request);
    
    private static partial Choice ToDomain(this ChoiceUpdateRequest request);
    
    private static partial ChoiceResponse ToContract(this Choice choice);
    
    private static partial Question ToDomain(this QuestionCreateRequest request);
    
    private static partial Question ToDomain(this QuestionUpdateRequest request);

    private static partial QuestionResponse ToContract(this Question question);
    
    public static partial Exam ToDomain(this ExamCreateRequest request);
    
    public static partial Exam ToDomain(this ExamUpdateRequest request);

    public static ExamResponse ToContract(this Exam exam)
    {
        var response = exam.ToContractInternal();

        response.MaxGradePoints = exam.MaxGradePoints;
        response.LastGottenGradePoints = exam.ExamSessions.FirstOrDefault()?.Grade ?? 0;
        response.Status = exam.UserExamStates.FirstOrDefault()?.Status ?? ExamStatus.NotTaken;
        
        return response;
    }
    
    private static partial ExamResponse ToContractInternal(this Exam exam);

    public static ExamSessionStateResponse ToContract(this ExamSessionState state)
    {
        return new ExamSessionStateResponse
        {
            Questions = state.Questions.Select(q => new QuestionIsAnswered
            {
                Id = q.QuestionId,
                IsAnswered = q.IsAnswered
            }).ToList(),
            CheckpointQuestionId = state.CheckpointQuestionId,
            StartDateUtc = state.StartDateUtc,
            DurationMinutes = state.DurationMinutes
        };
    }

    public static ExamSessionQuestionChoiceResponse ToContract(this ExamSessionQuestionChoice model)
    {
        return new ExamSessionQuestionChoiceResponse
        {
            Id = model.QuestionId,
            Text = model.Question.Text,
            Points = model.Question.Points,
            Image = model.Question.Image?.ToContract(),
            ChosenAnswer = model.ChoiceId,
            Choices = model.Question.Choices.Select(c => c.ToSessionContract()).ToList()
        };
    }
    
    private static partial SessionChoiceResponse ToSessionContract(this Choice choice);

    public static ExamSimpleResponse ToSimpleContract(this Exam exam)
    {
        var response = exam.ToSimpleContractInternal();

        response.QuestionsCount = exam.Questions.Count;

        return response;
    }
    
    private static partial ExamSimpleResponse ToSimpleContractInternal(this Exam exam);
}