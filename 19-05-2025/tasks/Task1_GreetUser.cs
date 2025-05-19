using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks.Properties
{
    class Task1_GreetUser
    {
        public static void Run(string? name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Console.WriteLine($"Hello, {name}!");
            }
            else
            {
                Console.WriteLine("Enter a proper name man! ");
            }
        }
    }
}
