using EventManagementAPI.Models;

namespace EventManagementAPI.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User> AddAsync(User user);
    Task<User?> UpdateAsync(User user);
    Task<bool> DeleteAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task UpdateCoinsAsync(Guid userId, int coins);

}
