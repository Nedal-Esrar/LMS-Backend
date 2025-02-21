namespace MLMS.API.Exams.Responses;

public class ExamSimpleResponse
{
    public int DurationMinutes { get; set; }
    
    public int PassThresholdPoints { get; set; }

    public int QuestionsCount { get; set; }
    
    public int MaxGradePoints { get; set; }
}