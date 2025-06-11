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
    {
        var events = await _eventService.GetAllEventsAsync(search, page, pageSize);

        if (!events.Any())
        {
            return Ok(new
            {
                Message = "No matching events found",
                Events = events
            });
        }

        return Ok(new
        {
            Events = events
        });
    }


    // G E T    E V E N T   B Y   Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventById(Guid id)
    {
        var evt = await _eventService.GetEventByIdAsync(id);
        // return evt == null ? NotFound() : Ok(evt);
        if (evt == null)
        {
            return NotFound(new { Message = $"Event with ID '{id}' not found." });
        }

        return Ok(evt);
    }
    
    // C R E A T E    E V E N T 
    [HttpPost]
    [Authorize(Roles = "Organizer")]
    public async Task<IActionResult> CreateEvent([FromForm] EventCreateDto dto)
    {
        var organizerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var newEvent = await _eventService.CreateEventAsync(dto, organizerId);
        return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id }, newEvent);
    }

    // U P D A T E    E V E N T 
    [HttpPut("{id}")]
    [Authorize(Roles = "Organizer")]
    public async Task<IActionResult> UpdateEvent(Guid id,[FromForm] EventUpdateDto dto)
    {
        var organizerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var updated = await _eventService.UpdateEventAsync(id, dto, organizerId);

        if (updated == null)
        {
            return NotFound(new { Message = $"Cannot update — event with ID '{id}' not found or you are unauthorized." });
        }
        return Ok(updated);
    }

    // D E L E T E    E V E N T
    [HttpDelete("{id}")]
    [Authorize(Roles = "Organizer")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var organizerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var deleted = await _eventService.DeleteEventAsync(id, organizerId);

        if (!deleted)
        {
            return NotFound(new { Message = $"Cannot delete — event with ID '{id}' not found or you are unauthorized." });
        }

        return NoContent();
    }
}
