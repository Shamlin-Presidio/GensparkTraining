namespace EventManagementAPI.Models;

public class Registration
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public Event? Event { get; set; }
    public Guid AttendeeId { get; set; }
    public User? Attendee { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
     public bool IsDeleted { get; set; } = false;
}
