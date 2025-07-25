namespace EventManagementAPI.Models;

public class Wallet
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Coins { get; set; }

    public User? User { get; set; }

    public List<WalletTransactions>? Transactions { get; set; }
}