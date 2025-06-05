using Microsoft.AspNetCore.Mvc;
using docuNotify.Models.DTOs;
using docuNotify.Interfaces;

namespace docuNotify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequestDto request)
        {
            if (!_authService.RegisterUser(request, out var error))
                return BadRequest(error);

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var token = _authService.LoginUser(request);

            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { token, username = request.Username });
        }
    }
}
