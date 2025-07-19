using EventManagementAPI.Models;

namespace EventManagementAPI.Interfaces;

public interface IWalletService
{
    public Task<Wallet> CreateNewWallet();
    public Task<int> AddCoinsToWallet(Guid userId, int coins, string type);

    public Task DeductCoinsFromWallet(Guid userId, int coins);

    public Task<int> GetCoinsInWallet(Guid userId);
}