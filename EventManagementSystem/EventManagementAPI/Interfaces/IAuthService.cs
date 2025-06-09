using EventManagementAPI.Models.DTOs.Auth;
using EventManagementAPI.Models.DTOs.User;

namespace EventManagementAPI.Interfaces;

public interface IAuthService
{
    Task<(string Token, string RefreshToken, UserResponseDto User)> SignUpAsync(UserCreateDto dto, IFormFile? profilePicture = null);
    Task<(string Token, string RefreshToken, UserResponseDto User)> LoginAsync(LoginRequestDto dto);
}
