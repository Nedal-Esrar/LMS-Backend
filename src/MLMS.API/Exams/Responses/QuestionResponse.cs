using MLMS.API.Files.Responses;

namespace MLMS.API.Exams.Responses;

public class QuestionResponse
{
    public long Id { get; set; }
    
    public string Text { get; set; }
    
    public int Points { get; set; }
    
    public FileResponse? Image { get; set; }

    public List<ChoiceResponse> Choices { get; set; } = [];
}