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
    [HttpGet]
    public async Task<IActionResult> GetMyRegistrations()
    {
        var attendeeId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var registrations = await _registrationService.GetAllRegistrationsForUserAsync(attendeeId);
        return Ok(registrations);
    }

    // R E G I ST E R    
    [HttpPost("{eventId}")]
    public async Task<IActionResult> Register(Guid eventId)
    {
        var attendeeId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var reg = await _registrationService.RegisterForEventAsync(eventId, attendeeId);
        return Ok(reg);
    }


    // C A N C E L   R E G I S T R A T I O N
    [HttpDelete("{registrationId}")]
    public async Task<IActionResult> Cancel(Guid registrationId)
    {
        var attendeeId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var cancelled = await _registrationService.CancelRegistrationAsync(registrationId, attendeeId);
        return cancelled ? NoContent() : NotFound();
    }
}
