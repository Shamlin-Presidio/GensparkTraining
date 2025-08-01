using ShopApi.Dtos;

namespace ShopApi.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetPagedAsync(int page, int pageSize, int? categoryId);
    Task<ProductDetailDto?> GetByIdAsync(int id);
    Task<ProductResponseDto> CreateAsync(ProductCreateDto dto);

}
