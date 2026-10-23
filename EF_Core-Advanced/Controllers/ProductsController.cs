using EF_Core_Advanced.Enum;
using EF_Core_Advanced.Products;
using EF_Core_Advanced.Products.DTO;
using EF_Core_Advanced.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.DTOs.Products;

namespace EF_Core_Advanced.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetProducts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _productService.GetProductsAsync(
                pageNumber,
                pageSize,
                cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            [FromBody] UpdateProductDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _productService.UpdateAsync(
                id,
                dto,
                cancellationToken);

            return result switch
            {
                UpdateProductResult.Updated =>
                    NoContent(),

                UpdateProductResult.NotFound =>
                    NotFound(new { message = "Product not found." }),

                UpdateProductResult.CategoryNotFound =>
                    BadRequest(new { message = "Category not found." }),

                UpdateProductResult.Conflict =>
                    Conflict(new
                    {
                        message = "The product was modified by another user. Reload it and try again."
                    }),

                _ => StatusCode(500)
            };
        }
    }
}

