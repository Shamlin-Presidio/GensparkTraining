using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class Task10_ValidateSudokuRow
    {
        public static void Run()
        {
            Console.WriteLine("Enter 9 numbers (1 to 9) separated by space:");

            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 9)
            {
                Console.WriteLine("Invalid input: exactly 9 numbers required.");
                return;
            }

            int[] row = new int[9];
            for (int i = 0; i < 9; i++)
            {
                if (!int.TryParse(parts[i], out row[i]) || row[i] < 1 || row[i] > 9)
                {
                    Console.WriteLine("Invalid input: all values must be integers from 1 to 9.");
                    return;
                }
            }

            if (IsValidSudokuRow(row))
                Console.WriteLine("The row is valid.");
            else
                Console.WriteLine("The row is invalid.");
        }

        private static bool IsValidSudokuRow(int[] row)
        {
            HashSet<int> seen = new HashSet<int>();
            foreach (int num in row)
            {
                if (!seen.Add(num))
                    return false;
            }
            return true;
        }
    }
}
