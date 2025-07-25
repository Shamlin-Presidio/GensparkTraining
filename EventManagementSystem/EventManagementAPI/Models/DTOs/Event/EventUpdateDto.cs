namespace EventManagementAPI.Models.DTOs.Event;

public class EventUpdateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? GoogleMapLink { get; set; }
    public string? OnlineMeetUrl { get; set; }
    public IFormFile? ImagePath { get; set; } 
}
