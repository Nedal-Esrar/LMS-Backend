namespace MLMS.Domain.Entities;

public class Attachment
{
    public string Name { get; set; }
    
    public byte[] File { get; set; }
    
    public string MediaType { get; set; }

    public string SubMediaType { get; set; }
}