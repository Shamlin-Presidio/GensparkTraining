using ShopApi.Models;

namespace ShopApi.Interfaces;

public interface INewsRepository : IRepository<News>
{
    Task<IEnumerable<News>> GetTopNewsAsync(int count);
}
