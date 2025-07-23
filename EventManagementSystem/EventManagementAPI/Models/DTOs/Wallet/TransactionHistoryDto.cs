namespace EventManagementAPI.Models.DTOs.Wallet;

public class TransactionHistoryDto
{
    public Guid Id { get; set; }
    public int Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
}