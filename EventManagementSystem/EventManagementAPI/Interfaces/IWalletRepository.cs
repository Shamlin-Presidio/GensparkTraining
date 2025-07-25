using EventManagementAPI.Models;

namespace EventManagementAPI.Interfaces;

public interface IWalletRepository
{
    public Task<IEnumerable<Wallet>> GetAllAsync();
    public Task<Wallet?> GetByIdAsync(Guid id);
    public Task<Wallet> AddAsync(Wallet wallet);
    public Task<Wallet?> UpdateAsync(Wallet wallet);
    public Task<Wallet?> GetByUserIdAsync(Guid id);
}