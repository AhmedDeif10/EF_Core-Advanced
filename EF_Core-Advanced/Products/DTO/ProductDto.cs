namespace EF_Core_Advanced.Products.DTO
{
    public record ProductDto(
        int Id,
        string Name,
        decimal Price,
        int CategoryId,
        string CategoryName,
        byte[] RowVersion);
}
