using ErrorOr;
using MLMS.Domain.Common.Models;

namespace MLMS.Domain.SectionParts;

public static class SectionPartErrors
{
    public static Error InvalidThresholdPoints =>
        Error.Validation(
            code: "SectionPart.InvalidThresholdPoints", 
            description: "Invalid threshold points value for exam.");

    public static Error NotFound => Error.NotFound(
        code: "SectionPart.NotFound",
        description: "Section part not found.");

    public static Error ChoicesCountNotEnough =>
        Error.Validation(
            code: "SectionPart.ChoicesCountNotEnough", 
            description: "Choices count must be at least 2.");

    public static Error QuestionWithMultipleCorrectChoices => Error.Validation(
        code: "SectionPart.QuestionWithMultipleCorrectChoices",
        description: "Question can have only one correct choice.");

    public static Error InvalidSectionPartStatusTransition => Error.Conflict(
        code: "SectionPart.InvalidSectionPartStatusTransition",
        description: "Invalid section part status transition.");
}