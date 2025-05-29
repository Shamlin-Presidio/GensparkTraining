using BankingApi.Models;
using BankingApi.Models.DTOs;

namespace BankingApi.Interfaces
{
    public interface IAccountService
    {
        Task<Account> CreateAccount(AccountCreateDto dto);
        Task<Account> PerformTransaction(TransactionDto dto);
    }
}