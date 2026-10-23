using System.ComponentModel.DataAnnotations;

namespace ProductsApi.DTOs.Products;

public record UpdateProductDto(
    [Required]
    [StringLength(150, MinimumLength = 2)]
    string Name,

    [Range(typeof(decimal), "0.01", "9999999999999999")]
    decimal Price,

    [Range(1, int.MaxValue)]
    int CategoryId,

    [Required]
    byte[] RowVersion);