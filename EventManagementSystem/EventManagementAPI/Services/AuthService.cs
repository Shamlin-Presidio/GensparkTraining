using BCrypt.Net;
using EventManagementAPI.Interfaces;
using EventManagementAPI.Models;
using EventManagementAPI.Models.DTOs.Auth;
using EventManagementAPI.Models.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace EventManagementAPI.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IWebHostEnvironment _env;

    public AuthService(
        IUserService userService,
        IUserRepository userRepository,
        IJwtService jwtService,
        IWebHostEnvironment env)
    {
        _userService = userService;
        _userRepository = userRepository;
        _jwtService = jwtService;
        _env = env;
    }

    public async Task<(string Token, UserResponseDto User)> SignUpAsync(UserCreateDto dto, IFormFile? profilePicture = null)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow
        };


        if (profilePicture != null)
        {
            var folder = Path.Combine("UploadedFiles", "Users");
            var extension = Path.GetExtension(profilePicture.FileName);
            var fileName = $"{user.Id}{extension}";
            var folderPath = Path.Combine(_env.ContentRootPath, folder);
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await profilePicture.CopyToAsync(stream);

            user.ProfilePicturePath = Path.Combine(folder, fileName).Replace("\\", "/");
        }

        await _userRepository.AddAsync(user);

        var responseDto = new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
        };

        var token = _jwtService.GenerateAccessToken(user);
        return (token, responseDto);
    }

    public async Task<(string Token, UserResponseDto User)> LoginAsync(LoginRequestDto dto)
    {
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Username == dto.Username && !u.IsDeleted);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var responseDto = new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };

        var token = _jwtService.GenerateAccessToken(user);
        return (token, responseDto);
    }
}
