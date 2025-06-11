using System.Security.Claims;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EventManagementAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // G E T    A L L    USERS
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _userService.GetAllUsersAsync());
    }

    // S E A R C H    USER
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    // R E G I S T E R   USER
    // [HttpPost]
    // public async Task<IActionResult> CreateUser(UserCreateDto dto)
    // {
    //     var user = await _userService.CreateUserAsync(dto);
    //     return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    // }

    // U P D A T E    USER
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromForm] UserUpdateDto dto)
    {
        // var updated = await _userService.UpdateUserAsync(id, dto);
        // return updated == null ? NotFound() : Ok(updated);

        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
        if (userIdClaim == null)
            return Unauthorized("User ID not found in token. Login to perform this action");

        var currentUserId = Guid.Parse(userIdClaim.Value);

        if (id != currentUserId)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new
            {
                message = "You are not authorized to update another user's information."
            });
        }

        var updated = await _userService.UpdateUserAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    // D E L E T E    USER
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        // var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
        // if (userIdClaim == null)
        //     return Unauthorized("User ID not found in token. Login to perform this action");

        // var currentUserId = Guid.Parse(userIdClaim.Value);

        // var result = await _userService.DeleteUserAsync(id, currentUserId);
        // if (!result)
        //     return NotFound();

        // return Ok("Your account has been deleted.");
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token. Login to perform this action.");

            var currentUserId = Guid.Parse(userIdClaim.Value);

            var result = await _userService.DeleteUserAsync(id, currentUserId);
            if (!result)
                return NotFound();

            return Ok("Your account has been deleted.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new
            {
                message = "You are not authorized to delete this account.",
                reason = ex.Message
            });
        }
    }
}