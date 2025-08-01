using ShopApi.Dtos;

namespace ShopApi.Services.Interfaces
{
    public interface IContactUsService
    {
        Task<IEnumerable<ContactUsResponseDto>> GetAllAsync();
        Task<ContactUsResponseDto?> GetByIdAsync(int id);
        Task<ContactUsResponseDto> CreateAsync(ContactUsRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
