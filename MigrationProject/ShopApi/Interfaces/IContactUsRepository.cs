using ShopApi.Models;

namespace ShopApi.Repositories.Interfaces
{
    public interface IContactUsRepository
    {
        Task<IEnumerable<ContactUs>> GetAllAsync();
        Task<ContactUs?> GetByIdAsync(int id);
        Task AddAsync(ContactUs contact);
        void Delete(ContactUs contact);
        Task SaveAsync();
    }
}
