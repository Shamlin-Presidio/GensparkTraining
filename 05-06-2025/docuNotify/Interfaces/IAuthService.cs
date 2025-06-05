using docuNotify.Models.DTOs;

namespace docuNotify.Interfaces
{
    public interface IAuthService
    {
        bool RegisterUser(RegisterRequestDto dto, out string error);
        string? LoginUser(LoginRequestDto dto);
    }
}