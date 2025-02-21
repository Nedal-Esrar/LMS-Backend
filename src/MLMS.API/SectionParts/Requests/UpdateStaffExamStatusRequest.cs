namespace MLMS.API.SectionParts.Requests;

public class UpdateStaffExamStatusRequest
{
    public List<(long QuestionId, long ChoiceId)> Answers { get; set; } = [];
}