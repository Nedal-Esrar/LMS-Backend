using Sieve.Models;

namespace MLMS.API.Common;

public static class RetrievalMapper
{
    public static SieveModel ToSieveModel(this RetrievalRequest request)
    {
        return new SieveModel
        {
            Filters = request.Filters,
            Sorts = request.Sorts,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}