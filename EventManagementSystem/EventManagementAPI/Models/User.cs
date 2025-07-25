namespace EventManagementAPI.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Attendee";
    public Guid WalletId { get; set; }
    public string? ProfilePicturePath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;

    public Wallet? Wallet { get; set; }
    public ICollection<Event>? OrganizedEvents { get; set; }
    public ICollection<Registration>? Registrations { get; set; }
}
