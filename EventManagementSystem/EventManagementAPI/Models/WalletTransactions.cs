namespace EventManagementAPI.Models;

public class WalletTransactions
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public int Amount { get; set; }
    public string Type { get; set; } = string.Empty; // Topup, Registration, Refund
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public Wallet? Wallet { get; set; }
}