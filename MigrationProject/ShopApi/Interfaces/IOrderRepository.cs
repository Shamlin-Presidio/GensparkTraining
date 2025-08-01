using ShopApi.Models;

namespace ShopApi.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    // Task AddAsync(Order order);
    Task<List<Order>> GetPagedAsync(int page, int pageSize);
    // Task<List<Order>> GetAllAsync();
    // Task<Order?> GetByIdAsync(int id);
    // Task AddAsync(Order order);
    // Task UpdateAsync(Order order);
    // Task DeleteAsync(Order order);
}
