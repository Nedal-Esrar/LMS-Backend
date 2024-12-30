using MLMS.API.Files.Responses;

namespace MLMS.API.Exams.Responses;

public class ExamSessionQuestionChoiceResponse
{
    public long Id { get; set; }
    
    public string Text { get; set; }
    
    public int Points { get; set; }
    
    public FileResponse? Image { get; set; }
    
    public List<SessionChoiceResponse> Choices { get; set; } = [];
    
    public long? ChosenAnswer { get; set; }
}