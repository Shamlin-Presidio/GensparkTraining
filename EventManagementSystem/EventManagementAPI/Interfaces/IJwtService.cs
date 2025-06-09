using EventManagementAPI.Models;

namespace EventManagementAPI.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
