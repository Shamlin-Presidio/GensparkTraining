using Microsoft.EntityFrameworkCore;
using BankingApi.Models;
using BankingApi.Interfaces;
using BankingApi.Models.DTOs;
using BankingApi.Misc;

namespace BankingApi.Services
{
    public class AccountServiceWithTransaction : IAccountService
    {
        private readonly BankingContext _context;
        private readonly AccountMapper _mapper;

        public AccountServiceWithTransaction(BankingContext context)
        {
            _context = context;
            _mapper = new AccountMapper();
        }

        // Account CREation
        public async Task<Account> CreateAccount(AccountCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var account = _mapper.MapCreateDtoToAccount(dto);
                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return account;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Basic transcation --> Deposit, Withdraw
        public async Task<Account> PerformTransaction(TransactionDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var account = await _context.Accounts.FindAsync(dto.AccountId);
                if (account == null) throw new Exception("Account not found");

                if (dto.Type == TransactionType.Deposit)
                    account.Balance += dto.Amount;
                else if (dto.Type == TransactionType.Withdraw)
                {
                    if (account.Balance < dto.Amount)
                        throw new Exception("Insufficient funds");
                    account.Balance -= dto.Amount;
                }

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return account;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}