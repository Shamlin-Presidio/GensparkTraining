using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class Task4_CheckCredentials
    {
        public static void Run()
        {
            int attempts = 0;
            while (attempts < 3)
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                if (username == "Admin" && password == "pass")
                {
                    Console.WriteLine("Login successful!");
                    return;
                }

                attempts++;
                Console.WriteLine("Invalid credentials.\n");
            }

            Console.WriteLine("Invalid attempts for 3 times. Exiting....");
        }
    }
}
