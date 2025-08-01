using ShopApi.Models;

namespace ShopApi.Interfaces;

public interface IModelRepository : IRepository<Model>
{
    Task<IEnumerable<Model>> GetAllWithProductsAsync();
}
