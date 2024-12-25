using MLMS.Domain.Exams;

namespace MLMS.Domain.SectionParts;

public class ExamState
{
    public ExamStatus Status { get; set; }

    public List<(Question, Choice)> CorrectAnswers { get; set; } = [];
}