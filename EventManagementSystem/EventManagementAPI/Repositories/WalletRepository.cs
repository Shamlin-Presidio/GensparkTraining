using EventManagementAPI.Data;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly AppDbContext _context;
    public WalletRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Wallet>> GetAllAsync()
    {
        return await _context.Wallets.ToListAsync();
    }

    public async Task<Wallet?> GetByIdAsync(Guid id)
    {
        return await _context.Wallets.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Wallet> AddAsync(Wallet wallet)
    {
        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();
        return wallet;
    }

    public async Task<Wallet?> UpdateAsync(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        await _context.SaveChangesAsync();
        return wallet;
    }

    public async Task<Wallet?> GetByUserIdAsync(Guid id)
    {
        return await _context.Wallets.Include(w => w.User).FirstOrDefaultAsync(w => w.User.Id == id);
    }
}