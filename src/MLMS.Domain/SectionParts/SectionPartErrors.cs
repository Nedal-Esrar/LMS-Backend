using ErrorOr;
using MLMS.Domain.Common.Models;

namespace MLMS.Domain.SectionParts;

public static class SectionPartErrors
{
    public static Error InvalidThresholdPoints =>
        Error.Validation("SectionPart.InvalidThresholdPoints", "Invalid threshold points value.");

    public static Error NotFound => Error.NotFound("SectionPart.NotFound");

    public static Error ChoicesCountNotEnough =>
        Error.Validation("SectionPart.ChoicesCountNotEnough", "Choices count must be at least 2.");

    public static Error QuestionWithMultipleCorrectChoices => Error.Validation(
        "SectionPart.QuestionWithMultipleCorrectChoices",
        "Question can have only one correct choice.");

    
    public static Error NotExam => Error.Conflict("SectionPart.NotExam", "Section part is not an exam.");
    public static Error NotAllQuestionsAnswered => Error.Validation("SectionPart.NotAllQuestionsAnswered", "Not all questions are answered.");

    public static Error QuestionsChoicesAssociationError =>
        Error.Conflict("SectionPart.QuestionsChoicesAssociationError", "Questions and choices association error.");
}