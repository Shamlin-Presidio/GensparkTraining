using System;

namespace SecureFileAccessProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            User adminUser = new User("admin", Role.Admin);
            User normalUser = new User("user", Role.User);
            User guestUser = new User("guest", Role.Guest);

            IFile adminFileProxy = new ProxyFile("SecretDocument.txt", adminUser);
            IFile userFileProxy = new ProxyFile("SecretDocument.txt", normalUser);
            IFile guestFileProxy = new ProxyFile("SecretDocument.txt", guestUser);

            Console.WriteLine($"User: {adminUser.Username} | Role: {adminUser.UserRole}");
            adminFileProxy.Read();
            Console.WriteLine();

            Console.WriteLine($"User: {normalUser.Username} | Role: {normalUser.UserRole}");
            userFileProxy.Read();
            Console.WriteLine();

            Console.WriteLine($"User: {guestUser.Username} | Role: {guestUser.UserRole}");
            guestFileProxy.Read();
            Console.WriteLine();
        }
    }
}
