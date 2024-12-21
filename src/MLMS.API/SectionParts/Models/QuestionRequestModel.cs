namespace MLMS.API.SectionParts.Models;

public class QuestionRequestModel
{
    public string Text { get; set; }
    
    public int Points { get; set; }

    public List<QuestionChoiceRequestModel> Choices { get; set; } = [];
}