namespace MLMS.Domain.Common.Models;

public class PaginatedList<TItem>
{
    public List<TItem> Items { get; init; } = [];
    
    public PaginationMetadata Metadata { get; init; }
}