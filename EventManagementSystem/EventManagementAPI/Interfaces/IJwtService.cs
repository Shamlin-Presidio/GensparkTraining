using System.Security.Claims;
using EventManagementAPI.Models;

namespace EventManagementAPI.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    public ClaimsPrincipal? GetPrincipalFromToken(string token);
}
