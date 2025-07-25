using EventManagementAPI.Data;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Repositories;

public class WalletTransactionsRepository : IWalletTransactionsRepository
{
    private readonly AppDbContext _context;
    public WalletTransactionsRepository(AppDbContext context) => _context = context;
    public async Task<WalletTransactions> AddAsync(WalletTransactions transaction)
    {
        _context.WalletTransactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<IEnumerable<WalletTransactions>> GetAllAsync()
    {
       return await _context.WalletTransactions.ToListAsync();
    }

    public async Task<List<WalletTransactions>> GetAllByWalletIdAsync(Guid walletId)
    {
        return await _context.WalletTransactions.Where(wt => wt.WalletId == walletId).ToListAsync();
    }

    public async Task<WalletTransactions?> GetByIdAsync(Guid id)
    {
        return await _context.WalletTransactions.FirstOrDefaultAsync(wt => wt.Id == id);
    }
}