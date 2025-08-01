using ShopApi.Dtos;

namespace ShopApi.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> GetPagedNewsAsync(int page, int pageSize);
         Task<IEnumerable<NewsResponseDto>> GetAllNewsAsync();
        Task<NewsResponseDto> GetNewsByIdAsync(int id);
        Task<NewsResponseDto> CreateNewsAsync(NewsCreateDto dto);
        Task<NewsResponseDto> UpdateNewsAsync(int id, NewsUpdateDto dto);
        Task<bool> DeleteNewsAsync(int id);
        Task<byte[]> ExportToCsvAsync();
        Task<byte[]> ExportToExcelAsync();
    }
}
