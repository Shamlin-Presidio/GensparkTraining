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
        var updated = await _userService.UpdateUserAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    // D E L E T E    USER
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var deleted = await _userService.DeleteUserAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}