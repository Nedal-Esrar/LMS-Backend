using ErrorOr;

namespace MLMS.Domain.Exams;

public static class ExamErrors
{
    public static Error SessionAlreadyStarted => Error.Conflict(
        code: "Exams.SessionAlreadyStarted",
        description: "A session for this exam has already started");
    
    public static Error NotFound => Error.NotFound(
        code: "Exams.NotFound",
        description: "Exam not found");
    
    public static Error QuestionNotFound => Error.NotFound(
        code: "Exams.Questions.NotFound",
        description: "The question specified on this exam was not found.");
    
    public static Error ChoiceNotFound => Error.NotFound(
        code: "Exams.Questions.Choices.NotFound",
        description: "Choice not found for the question specified on this exam.");
    
    public static Error SessionEnded => Error.Conflict(
        code: "Exams.SessionEnded",
        description: "The session for this exam has already ended.");
}