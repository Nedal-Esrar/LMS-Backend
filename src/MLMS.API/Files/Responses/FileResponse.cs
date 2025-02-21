namespace MLMS.API.Files.Responses;

public class FileResponse
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string ContentType { get; set; }
    
    public string Extension { get; set; }
    
    public string Path { get; set; }
}