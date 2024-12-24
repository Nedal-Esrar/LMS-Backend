namespace MLMS.API.SectionParts.Models;

public class QuestionContractModel
{
    public string Text { get; set; }
    
    public int Points { get; set; }

    public List<ChoiceContractModel> Choices { get; set; } = [];
}