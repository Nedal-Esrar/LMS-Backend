using ErrorOr;
using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Exams;

public static class ExamErrors
{
    public static Error SessionAlreadyStarted => Error.Conflict("Session already started");
    
    public static Error NotFound => Error.NotFound("Exam not found");
    
    public static Error QuestionNotFound => Error.NotFound("Question not found");
    
    public static Error ChoiceNotFound => Error.NotFound("Choice not found");
}