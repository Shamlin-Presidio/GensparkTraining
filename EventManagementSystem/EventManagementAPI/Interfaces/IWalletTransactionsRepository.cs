using EventManagementAPI.Models;

namespace EventManagementAPI.Interfaces;

public interface IWalletTransactionsRepository
{
    public Task<IEnumerable<WalletTransactions>> GetAllAsync();
    public Task<WalletTransactions?> GetByIdAsync(Guid id);
    public Task<WalletTransactions> AddAsync(WalletTransactions transaction);
    public Task<List<WalletTransactions>> GetAllByWalletIdAsync(Guid id);
}