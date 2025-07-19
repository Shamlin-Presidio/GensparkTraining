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
    // private readonly IBlobService _blobService;
    private readonly IWalletService _walletService;
    private readonly IWebHostEnvironment _env;

    public AuthService(
        IUserService userService,
        IUserRepository userRepository,
        IJwtService jwtService,
        IWebHostEnvironment env,
        IWalletService walletService
        // ,IBlobService blobService
        )
    {
        _userService = userService;
        _userRepository = userRepository;
        _jwtService = jwtService;
        _walletService = walletService;
        // _blobService = blobService;
        _env = env;
    }

    public async Task<(string Token, string RefreshToken, UserResponseDto User)> SignUpAsync(UserCreateDto dto, IFormFile? profilePicture = null)
    {
        var validRoles = new[] { "Organizer", "Attendee" };
        if (!validRoles.Contains(dto.Role, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("Role must be either 'Organizer' or 'Attendee'.");

        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
            throw new InvalidOperationException("An account with this email already exists.");

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

            // var extension = Path.GetExtension(profilePicture.FileName);
            // var blobFileName = $"Users/{user.Id}{extension}";
            // var blobUrl = await _blobService.UploadAsync(profilePicture, blobFileName);
            // user.ProfilePicturePath = blobUrl;
        }

        var wallet = await _walletService.CreateNewWallet();
        user.WalletId = wallet.Id;

        await _userRepository.AddAsync(user);

        var responseDto = new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            ProfilePicturePath = user.ProfilePicturePath,
        };

        var token = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken(user);
        return (token, refreshToken, responseDto);

    }


    public async Task<(string Token, string RefreshToken, UserResponseDto User)> LoginAsync(LoginRequestDto dto)
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
            Role = user.Role,
            ProfilePicturePath = user.ProfilePicturePath,
        };

        var token = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken(user);
        return (token, refreshToken, responseDto);
    }
}
