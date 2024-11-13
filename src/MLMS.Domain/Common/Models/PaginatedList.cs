namespace MLMS.Domain.Common.Models;

public class PaginatedList<TItem>
{
    public List<TItem> Items { get; set; }
    
    public PaginationMetadata Metadata { get; set; }
}