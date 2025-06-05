using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using docuNotify.Interfaces;
using docuNotify.Models;
using docuNotify.Models.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace docuNotify.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public bool RegisterUser(RegisterRequestDto dto, out string error)
        {
            error = "";

            if (_userRepository.UserExists(dto.Username))
            {
                error = "User already exists";
                return false;
            }

            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password,
                Role = dto.Role
            };

            _userRepository.AddUser(user);
            _userRepository.SaveChanges();

            return true;
        }

        public string? LoginUser(LoginRequestDto dto)
        {
            var user = _userRepository.GetUser(dto.Username, dto.Password);

            if (user == null)
                return null;

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is a dummy key that is used for development. Ensure you replace this"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "docuNotify",
                audience: "docuNotifyUser",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}