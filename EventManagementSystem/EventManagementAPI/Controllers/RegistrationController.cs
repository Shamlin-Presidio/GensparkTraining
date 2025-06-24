using EventManagementAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Attendee")]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    public RegistrationController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    // G E T   R E G I S T E R E D    E V E N T S
    [HttpGet("GetMyRegistrations")]
    public async Task<IActionResult> GetMyRegistrations()
    {
        var attendeeId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var registrations = await _registrationService.GetAllRegistrationsForUserAsync(attendeeId);
        return Ok(registrations);
    }

    // R E G I ST E R    
    // [HttpPost("{eventId}")]
    // public async Task<IActionResult> Register(Guid eventId)
    // {
    //     var attendeeId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    //     var reg = await _registrationService.RegisterForEventAsync(eventId, attendeeId);
    //     return Ok(reg);
    // }

    [HttpPost("Register/{eventId}")]
    public async Task<IActionResult> Register(Guid eventId)
    {
        var attendeeId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            var registration = await _registrationService.RegisterForEventAsync(eventId, attendeeId);
            if (registration == null)
                return BadRequest(new { Message = "Registration failed. Possibly event not found or already registered." });

            return CreatedAtAction(nameof(GetMyRegistrations), null, registration);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }


    // C A N C E L   R E G I S T R A T I O N
    [HttpDelete("Cancel/{registrationId}")]
    public async Task<IActionResult> Cancel(Guid registrationId)
    {
        var attendeeId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var cancelled = await _registrationService.CancelRegistrationAsync(registrationId, attendeeId);
        if (!cancelled)
            return NotFound(new { Message = "Registration not found or you are not authorized to cancel it." });

        return Ok(new { Message = "Registration cancelled successfully." });
    }

    //   G E T   No. R E G I S T R A T I O N S    F O R    E V E N T
    [HttpGet("Count/{eventId}")]
    public async Task<IActionResult> GetRegistrationCount(Guid eventId)
    {
        var count = await _registrationService.GetRegistrationCountAsync(eventId);
        return Ok(new { Count = count });
    }

}
