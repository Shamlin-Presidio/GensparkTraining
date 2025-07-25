namespace EventManagementAPI.Models.DTOs.Wallet;

public class EventWithdrawResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime EndTime { get; set; }
    public int TotalRegistrations { get; set; }
    public int RegisteredCoins { get; set; }
    public bool IsWithdrawn { get; set; }
}