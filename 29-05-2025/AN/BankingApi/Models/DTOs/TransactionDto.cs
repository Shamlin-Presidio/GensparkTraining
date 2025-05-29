namespace BankingApi.Models.DTOs
{
    public class TransactionDto
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
    }
}