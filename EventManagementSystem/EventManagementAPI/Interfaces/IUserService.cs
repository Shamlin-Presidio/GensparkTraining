using EventManagementAPI.Models.DTOs.User;

namespace EventManagementAPI.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> GetUserByIdAsync(Guid id);
    // Task<UserResponseDto> CreateUserAsync(UserCreateDto dto);
    Task<UserResponseDto?> UpdateUserAsync(Guid id, UserUpdateDto dto);
    Task<bool> DeleteUserAsync(Guid id, Guid currentUserId);

    Task<int> GetCoinsAsync(Guid userId);
    Task UpdateCoinsAsync(Guid userId, int coins);
}
