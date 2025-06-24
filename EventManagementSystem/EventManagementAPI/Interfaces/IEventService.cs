using EventManagementAPI.Models.DTOs.Event;

namespace EventManagementAPI.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventResponseDto>> GetAllEventsAsync(string? search = null, int page = 1, int pageSize = 10);
    Task<EventResponseDto?> GetEventByIdAsync(Guid id);
    Task<EventResponseDto> CreateEventAsync(EventCreateDto dto, Guid organizerId);
    Task<EventResponseDto?> UpdateEventAsync(Guid id, EventUpdateDto dto, Guid organizerId);
    Task<bool> DeleteEventAsync(Guid id, Guid organizerId);
    Task<IEnumerable<EventResponseDto>> GetEventsByOrganizerAsync(Guid organizerId);

}
