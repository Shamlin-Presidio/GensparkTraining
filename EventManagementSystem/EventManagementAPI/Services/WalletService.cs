using System.Threading.Tasks;
using EventManagementAPI.Data;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs.Wallet;

namespace EventManagementAPI.Services;

public class WalletService : IWalletService
{
    private readonly AppDbContext _context;
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletTransactionsRepository _walletTransactionsRepository;

    public WalletService(AppDbContext context, IWalletRepository walletRepository, IWalletTransactionsRepository walletTransactionsRepository)
    {
        _context = context;
        _walletRepository = walletRepository;
        _walletTransactionsRepository = walletTransactionsRepository;
    }

    public async Task<int> AddCoinsToWallet(Guid userId, int coins, string type)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet == null)
                throw new Exception("User not found");

            var walletTransaction = new WalletTransactions()
            {
                Amount = coins,
                Type = type,
                WalletId = wallet.Id
            };

            await _walletTransactionsRepository.AddAsync(walletTransaction);
            wallet.Coins += coins;
            wallet = await _walletRepository.UpdateAsync(wallet);
            await transaction.CommitAsync();
            return wallet.Coins;
        }
        catch (Exception ex)
        {

            await transaction.RollbackAsync();
            throw ex;
        };
    }

    public async Task<Wallet> CreateNewWallet()
    {
        var wallet = new Wallet();
        wallet = await _walletRepository.AddAsync(wallet);
        return wallet;
    }

    public async Task<int> DeductCoinsFromWallet(Guid userId, int coins, string type)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet == null)
                throw new Exception("User not found");

            if (wallet.Coins < coins)
                throw new Exception("Insufficient coins");

            wallet.Coins -= coins;

            var walletTransaction = new WalletTransactions()
            {
                Amount = coins,
                Type = type,
                WalletId = wallet.Id
            };

            await _walletTransactionsRepository.AddAsync(walletTransaction);
            wallet = await _walletRepository.UpdateAsync(wallet);
            await transaction.CommitAsync();
            return wallet.Coins;
        }
        catch (Exception ex)
        {

            await transaction.RollbackAsync();
            throw ex;
        };
    }

    public async Task<int> GetCoinsInWallet(Guid userId)
    {
        var wallet = await _walletRepository.GetByUserIdAsync(userId);
        if (wallet == null)
            throw new Exception("User not found");

        return wallet.Coins;
    }

    public async Task<List<TransactionHistoryDto>> GetWalletTransactionHistory(Guid userId)
    {
        var wallet = await _walletRepository.GetByUserIdAsync(userId);
        if (wallet == null)
            throw new Exception("User not found");

        var transactions = await _walletTransactionsRepository.GetAllAsync();
        var walletTransactions = transactions.Where(wt => wt.WalletId == wallet.Id).Select(wt => new TransactionHistoryDto()
        {
            Id = wt.Id,
            Amount = wt.Amount,
            Date = wt.Date,
            Type = wt.Type
        });

        return walletTransactions.OrderByDescending(wt => wt.Date).ToList();
    }
}