namespace MLMS.Domain.Common.Models;

public class PaginationMetadata
{
    public int TotalItems { get; set; }
    
    public int PageSize { get; set; }
    
    public int Page { get; set; }
    
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}