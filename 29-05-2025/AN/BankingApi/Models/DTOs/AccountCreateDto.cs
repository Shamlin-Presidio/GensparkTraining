namespace BankingApi.Models.DTOs
{
    public class AccountCreateDto
    {
        public string HolderName { get; set; }
        public decimal InitialDeposit { get; set; }
    }
}