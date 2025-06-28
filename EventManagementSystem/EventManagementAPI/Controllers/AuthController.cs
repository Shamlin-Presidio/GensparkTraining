using System.Security.Claims;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models.DTOs.Auth;
using EventManagementAPI.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;

    public AuthController(IAuthService authService, IJwtService jwtService, IUserRepository userRepository)
    {
        _authService = authService;
        _jwtService = jwtService;
        _userRepository = userRepository;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromForm] UserCreateDto dto, IFormFile? profilePicture)
    {
        try
        {
            var (accessToken, refreshToken, user) = await _authService.SignUpAsync(dto, profilePicture);
            return Ok(new { accessToken, refreshToken, user });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }

    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        try
        {
            var (accessToken, refreshToken, user)= await _authService.LoginAsync(dto);
            return Ok(new { accessToken, refreshToken, user });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { Message = "Invalid username or password." });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto dto)
    {
        var principal = _jwtService.GetPrincipalFromToken(dto.RefreshToken);
        if (principal == null)
            return Unauthorized(new { Message = "Invalid refresh token." });

        var tokenType = principal.FindFirst("token_type")?.Value;
        if (tokenType != "refresh")
            return Unauthorized(new { Message = "Invalid token type." });

        var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || user.IsDeleted)
            return Unauthorized(new { Message = "User not found or inactive." });

        var newAccessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken(user);

        return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
    }
}
