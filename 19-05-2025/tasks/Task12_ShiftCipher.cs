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

            Console.Write("Enter the shift value (integer): ");
            if (!int.TryParse(Console.ReadLine(), out int shift))
            {
                Console.WriteLine("Invalid shift value.");
                return;
            }

            string encrypted = Encrypt(input, shift);
            string decrypted = Decrypt(encrypted, shift);

            Console.WriteLine($"Encrypted: {encrypted}");
            Console.WriteLine($"Decrypted: {decrypted}");
        }

        private static string Encrypt(string message, int shift)
        {
            char[] result = new char[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                char c = message[i];
                result[i] = (char)('a' + (c - 'a' + shift + 26) % 26);
            }
            return new string(result);
        }

        private static string Decrypt(string message, int shift)
        {
            char[] result = new char[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                char c = message[i];
                result[i] = (char)('a' + (c - 'a' - shift + 26) % 26);
            }
            return new string(result);
        }
    }
}
