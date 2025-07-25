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
    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _userService.GetAllUsersAsync());
    }

    // S E A R C H    USER
    [HttpGet("GetUserById/{id}")]
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
    [HttpPut("UpdateUser/{id}")]
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
    [HttpDelete("DeleteUser/{id}")]
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

            return Ok(new { message = "Your account has been deleted." });
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


    // C O I N S

    // GET: api/User/{id}/coins
    [HttpGet("{id}/coins")]
    public async Task<IActionResult> GetCoins(Guid id)
    {
        try
        {
            int coins = await _userService.GetCoinsAsync(id);
            return Ok(coins);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    // PUT: api/User/{id}/coins
    [HttpPut("{id}/coins")]
    public async Task<IActionResult> UpdateCoins(Guid id, [FromQuery] int coins)
    {
        await _userService.UpdateCoinsAsync(id, coins);
        return NoContent();
    }
    
    // PUT: api/User/{id}/coins
    [HttpPut("{id}/deduct-coin")]
    public async Task<IActionResult> DeductCoin(Guid id)
    {

        try
        {
            int updatedCoins = await _userService.DeductCoinAsync(id);
            return Ok(updatedCoins);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}