using ShopApi.Dtos;

namespace ShopApi.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderResponseDto>> GetPagedOrdersAsync(int page, int pageSize);
    Task<OrderResponseDto?> GetByIdAsync(int id);
    Task<OrderResponseDto> CreateAsync(OrderCreateDto dto);
    Task<bool> UpdateAsync(int id, OrderUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<byte[]> ExportToPdfAsync();
}
