using MLMS.Domain.Questions;

namespace MLMS.Domain.SectionParts;

public class ExamState
{
    public ExamStatus Status { get; set; }

    public List<(Question, Choice)> CorrectAnswers { get; set; } = [];
}