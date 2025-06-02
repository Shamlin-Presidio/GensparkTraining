using FirstApi.Models;

namespace FirstApi.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}