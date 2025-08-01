using ShopApi.Models;

namespace ShopApi.Interfaces;

public interface IOrderDetailRepository : IRepository<OrderDetail>
{
    Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId);
}
