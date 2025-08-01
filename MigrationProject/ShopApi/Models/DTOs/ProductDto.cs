namespace ShopApi.Dtos;

public class ProductResponseDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
}

public class ProductDetailDto : ProductResponseDto
{
    public string? CategoryName { get; set; }
}
public class ProductCreateDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public int ColorId { get; set; }
}

