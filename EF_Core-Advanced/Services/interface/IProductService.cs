using EF_Core_Advanced.Enum;
using EF_Core_Advanced.Products;
using EF_Core_Advanced.Products.DTO;
using ProductsApi.DTOs.Products;

namespace EF_Core_Advanced.Services.Interface
{
    public interface IProductService
    {

        Task<PagedResult<ProductDto>> GetProductsAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<UpdateProductResult> UpdateAsync(
            int id,
            UpdateProductDto dto,
            CancellationToken cancellationToken);
    }
}
