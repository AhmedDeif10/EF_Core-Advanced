using EF_Core_Advanced.Enum;
using EF_Core_Advanced.Products;
using EF_Core_Advanced.Products.DTO;
using EF_Core_Advanced.Services.Interface;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.DTOs.Products;

namespace EF_Core_Advanced.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProductDto>> GetProductsAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = _context.Products.AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto(
                    p.Id,
                    p.Name,
                    p.Price,
                    p.CategoryId,
                    p.Category.Name,
                    p.RowVersion))
                .ToListAsync(cancellationToken);

            return new PagedResult<ProductDto>(
                items,
                pageNumber,
                pageSize,
                totalCount);
        }

        public async Task<UpdateProductResult> UpdateAsync(
            int id,
            UpdateProductDto dto,
            CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(
                    p => p.Id == id,
                    cancellationToken);

            if (product is null)
                return UpdateProductResult.NotFound;

            var categoryExists = await _context.Categories
                .AnyAsync(
                    c => c.Id == dto.CategoryId,
                    cancellationToken);

            if (!categoryExists)
                return UpdateProductResult.CategoryNotFound;

           
            _context.Entry(product)
                .Property(p => p.RowVersion)
                .OriginalValue = dto.RowVersion;

            
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);

                return UpdateProductResult.Updated;
            }
            catch (DbUpdateConcurrencyException)
            {
                return UpdateProductResult.Conflict;
            }
        }
    }
}
