using ShopApi.Models;

namespace ShopApi.Repositories.Interfaces
{
    public interface IColorRepository
    {
        Task<IEnumerable<Color>> GetAllAsync();
        Task<Color?> GetByIdAsync(int id);
        Task AddAsync(Color color);
        void Update(Color color);
        void Delete(Color color);
        Task SaveAsync();
    }
}
