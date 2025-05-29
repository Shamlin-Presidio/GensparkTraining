using BankingApi.Models;
using BankingApi.Models.DTOs;

namespace BankingApi.Misc
{
    public class AccountMapper
    {
        public Account MapCreateDtoToAccount(AccountCreateDto dto)
        {
            return new Account
            {
                HolderName = dto.HolderName,
                Balance = dto.InitialDeposit
            };
        }
    }

}