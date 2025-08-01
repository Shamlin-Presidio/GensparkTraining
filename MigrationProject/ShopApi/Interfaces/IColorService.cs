using ShopApi.Dtos;

namespace ShopApi.Services.Interfaces
{
    public interface IColorService
    {
        Task<IEnumerable<ColorResponseDto>> GetAllAsync();
        Task<ColorResponseDto?> GetByIdAsync(int id);
        Task<ColorResponseDto> CreateAsync(ColorRequestDto dto);
        Task<ColorResponseDto?> UpdateAsync(int id, ColorRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
