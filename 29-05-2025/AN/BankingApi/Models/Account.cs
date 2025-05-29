namespace BankingApi.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string HolderName { get; set; }
        public decimal Balance { get; set; }
    }
}