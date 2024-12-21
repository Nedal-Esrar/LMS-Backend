namespace MLMS.Domain.Common.Models;

public class PaginationMetadata
{
    public int TotalItems { get; init; }
    
    public int PageSize { get; init; }
    
    public int Page { get; init; }
    
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}