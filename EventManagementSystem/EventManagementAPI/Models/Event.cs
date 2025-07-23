namespace EventManagementAPI.Models;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int RegistrationFee { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? GoogleMapLink { get; set; }        
    public string? OnlineMeetUrl { get; set; }        
    public string? ImagePath { get; set; }            
    public Guid OrganizerId { get; set; }
    public User? Organizer { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public bool IsWithdrawn { get; set; } = false;

    public ICollection<Registration>? Registrations { get; set; }
}
