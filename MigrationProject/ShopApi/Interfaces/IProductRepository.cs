using ShopApi.Models;

namespace ShopApi.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    // Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
    Task<List<Product>> GetPagedAsync(int page, int pageSize, int? categoryId);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> AddAsync(Product product); 

}
