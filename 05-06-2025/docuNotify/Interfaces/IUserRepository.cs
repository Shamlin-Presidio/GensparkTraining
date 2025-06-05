using docuNotify.Models;

namespace docuNotify.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(string username);
        User? GetUser(string username, string password);
        void AddUser(User user);
        void SaveChanges();
    }
}