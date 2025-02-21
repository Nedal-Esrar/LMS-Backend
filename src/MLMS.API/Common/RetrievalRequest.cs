namespace MLMS.API.Common;

public class RetrievalRequest
{
    public string Filters { get; set; } = string.Empty;

    public string Sorts { get; set; } = string.Empty;

    public int Page { get; set; } = 1;
    
    public int PageSize { get; set; } = 15;
}