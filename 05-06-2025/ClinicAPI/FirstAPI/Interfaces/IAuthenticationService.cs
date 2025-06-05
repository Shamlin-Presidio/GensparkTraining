
using FirstApi.Models.DTOs;

namespace FirstApi.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<UserLoginResponse> Login(UserLoginRequest user);
    }
}