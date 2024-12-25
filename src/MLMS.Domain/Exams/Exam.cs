using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Exams;

public class Exam : EntityBase<long>
{
    public int DurationMinutes { get; set; }

    public List<Question> Questions { get; set; } = [];
}