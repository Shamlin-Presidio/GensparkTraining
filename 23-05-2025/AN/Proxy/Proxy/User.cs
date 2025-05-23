namespace SecureFileAccessProxy
{
    public class User
    {
        public string Username { get; }
        public Role UserRole { get; }

        public User(string username, Role role)
        {
            Username = username;
            UserRole = role;
        }
    }
}
