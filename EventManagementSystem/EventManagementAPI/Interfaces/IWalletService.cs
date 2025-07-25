using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs.Wallet;

namespace EventManagementAPI.Interfaces;

public interface IWalletService
{
    public Task<Wallet> CreateNewWallet();
    public Task<int> AddCoinsToWallet(Guid userId, int coins, string type);

    public Task<int> DeductCoinsFromWallet(Guid userId, int coins, string type);

    public Task<int> GetCoinsInWallet(Guid userId);
    public Task<List<TransactionHistoryDto>> GetWalletTransactionHistory(Guid userId);
    Task<IEnumerable<EventWithdrawResponseDto>> GetEventWithdrawDetails(Guid organizerId);
    Task<bool> WithdrawEventCoins(Guid eventId, Guid organizerId);
}