using EF_Core_Advanced.Products;

namespace EF_Core_Advanced.Products
{
    public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount);
}

