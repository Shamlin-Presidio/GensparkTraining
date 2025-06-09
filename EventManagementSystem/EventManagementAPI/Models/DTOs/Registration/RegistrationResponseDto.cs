namespace EventManagementAPI.Models.DTOs.Registration;

public class RegistrationResponseDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
}
