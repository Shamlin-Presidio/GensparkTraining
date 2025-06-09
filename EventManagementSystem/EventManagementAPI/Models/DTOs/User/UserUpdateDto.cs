namespace EventManagementAPI.Models.DTOs.User;

public class UserUpdateDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public IFormFile? ProfilePicture { get; set; }
}
