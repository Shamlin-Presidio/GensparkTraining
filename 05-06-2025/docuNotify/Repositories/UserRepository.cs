using docuNotify.Contexts;
using docuNotify.Interfaces;
using docuNotify.Models;

namespace docuNotify.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DocuNotifyContext _context;

        public UserRepository(DocuNotifyContext context)
        {
            _context = context;
        }

        public bool UserExists(string username) =>
            _context.Users.Any(u => u.Username == username);

        public User? GetUser(string username, string password) =>
            _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);

        public void AddUser(User user) =>
            _context.Users.Add(user);

        public void SaveChanges() =>
            _context.SaveChanges();
    }
}