using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class Task12_ShiftCipher
    {
        public static void Run()
        {
            Console.Write("Enter a message (lowercase only, no spaces or symbols): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input) || !input.All(char.IsLower))
            {
                Console.WriteLine("Invalid input. Only lowercase letters without spaces are allowed.");
                return;
            }

            string encrypted = Encrypt(input);
            string decrypted = Decrypt(encrypted);

            Console.WriteLine($"Encrypted: {encrypted}");
            Console.WriteLine($"Decrypted: {decrypted}");
        }

        private static string Encrypt(string message)
        {
            char[] result = new char[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                char c = message[i];
                result[i] = (char)('a' + (c - 'a' + 3) % 26);
            }
            return new string(result);
        }

        private static string Decrypt(string message)
        {
            char[] result = new char[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                char c = message[i];
                result[i] = (char)('a' + (c - 'a' - 3 + 26) % 26);
            }
            return new string(result);
        }
    }
}
