using MLMS.Domain.Common.Models;

namespace MLMS.API.Common;

public static class PaginationExtensions
{
    public static PaginatedList<TContract> ToContractPaginatedList<TItem, TContract>(
        this PaginatedList<TItem> paginatedList,
        Func<TItem, TContract> mappingFunc)
    {
        return new PaginatedList<TContract>
        {
            Items = paginatedList.Items.Select(mappingFunc).ToList(),
            Metadata = paginatedList.Metadata
        };
    }
}