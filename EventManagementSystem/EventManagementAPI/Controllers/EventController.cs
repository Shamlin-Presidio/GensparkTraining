using EventManagementAPI.Interfaces;
using EventManagementAPI.Models.DTOs.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }


    // G E T    A L L     E V E N T S 
    [HttpGet]
    public async Task<IActionResult> GetAllEvents([FromQuery] string? search, int page = 1, int pageSize = 10)
        => Ok(await _eventService.GetAllEventsAsync(search, page, pageSize));


    // G E T    E V E N T   B Y   Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventById(Guid id)
    {
        var evt = await _eventService.GetEventByIdAsync(id);
        return evt == null ? NotFound() : Ok(evt);
    }
    
    // C R E A T E    E V E N T 
    [HttpPost]
    [Authorize(Roles = "Organizer")]
    public async Task<IActionResult> CreateEvent(EventCreateDto dto)
    {
        var organizerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var newEvent = await _eventService.CreateEventAsync(dto, organizerId);
        return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id }, newEvent);
    }

    // U P D A T E    E V E N T 
    [HttpPut("{id}")]
    [Authorize(Roles = "Organizer")]
    public async Task<IActionResult> UpdateEvent(Guid id, EventUpdateDto dto)
    {
        var organizerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var updated = await _eventService.UpdateEventAsync(id, dto, organizerId);
        return updated == null ? NotFound() : Ok(updated);
    }

    // D E L E T E    E V E N T
    [HttpDelete("{id}")]
    [Authorize(Roles = "Organizer")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var organizerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var deleted = await _eventService.DeleteEventAsync(id, organizerId);
        return deleted ? NoContent() : NotFound();
    }
}
