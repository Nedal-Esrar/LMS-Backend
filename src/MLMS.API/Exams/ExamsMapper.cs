using MLMS.API.Exams.Requests;
using MLMS.API.Exams.Responses;
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
    
    public static partial ExamResponse ToContract(this Exam exam);
}