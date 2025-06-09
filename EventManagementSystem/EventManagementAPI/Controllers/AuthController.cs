using EventManagementAPI.Interfaces;
using EventManagementAPI.Models.DTOs.Auth;
using EventManagementAPI.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromForm] UserCreateDto dto, IFormFile? profilePicture)
    {
        var (token, user) = await _authService.SignUpAsync(dto, profilePicture);
        return Ok(new { token, user });
    }
    

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        try
        {
            var (token, user) = await _authService.LoginAsync(dto);
            return Ok(new { token, user });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid username or password");
        }
    }
}
