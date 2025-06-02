
using FirstApi.Models.DTOs.DoctorSpecialities;

namespace FirstApi.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<UserLoginResponse> Login(UserLoginRequest user);
    }
}