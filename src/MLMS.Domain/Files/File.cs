using MLMS.Domain.Common.Models;

namespace MLMS.Domain.Files;

public class File : EntityBase<Guid>
{
    public string Name { get; set; }
    
    public string ContentType { get; set; }
    
    public string Extension { get; set; }
    
    public string Path { get; set; }
}