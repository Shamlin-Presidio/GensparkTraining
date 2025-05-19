using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class Task11_ValidateSudokuBoard
    {
        public static void Run()
        {
            int[,] board = new int[9, 9];
            Console.WriteLine("Enter 9 rows of the Sudoku board (9 space-separated numbers each, 1–9):");

            for (int i = 0; i < 9; i++)
            {
                string? rowInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(rowInput))
                {
                    Console.WriteLine("Invalid input.");
                    return;
                }

                string[] parts = rowInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 9)
                {
                    Console.WriteLine("Each row must contain exactly 9 numbers.");
                    return;
                }

                for (int j = 0; j < 9; j++)
                {
                    if (!int.TryParse(parts[j], out board[i, j]) || board[i, j] < 1 || board[i, j] > 9)
                    {
                        Console.WriteLine("Invalid input: all values must be integers from 1 to 9.");
                        return;
                    }
                }
            }

            bool allValid = true;

            // Check rows
            for (int i = 0; i < 9; i++)
            {
                if (!IsValidGroup(GetRow(board, i)))
                {
                    Console.WriteLine($"Row {i + 1} is invalid.");
                    allValid = false;
                }
            }

            // Check columns
            for (int j = 0; j < 9; j++)
            {
                if (!IsValidGroup(GetColumn(board, j)))
                {
                    Console.WriteLine($"Column {j + 1} is invalid.");
                    allValid = false;
                }
            }

            // Check 3x3 boxes
            for (int row = 0; row < 9; row += 3)
            {
                for (int col = 0; col < 9; col += 3)
                {
                    if (!IsValidGroup(GetBox(board, row, col)))
                    {
                        Console.WriteLine($"Box starting at ({row + 1},{col + 1}) is invalid.");
                        allValid = false;
                    }
                }
            }

            if (allValid)
            {
                Console.WriteLine("The Sudoku board is valid!");
            }
            else
            {
                Console.WriteLine("The Sudoku board is invalid.");
            }
        }

        // Validates 9 integers, it can be row, column or 9 elemnets of a 3*3 grid
        // just provide 9 elements, it checks them!
        private static bool IsValidGroup(int[] group)
        {
            HashSet<int> seen = new HashSet<int>();
            foreach (int num in group)
            {
                if (num < 1 || num > 9 || !seen.Add(num))
                    return false;
            }
            return true;
        }

        // Functions to extract row, column and subgrids as an array of 9 elements

        // Get a row in an array
        private static int[] GetRow(int[,] board, int rowIndex)
        {
            int[] row = new int[9];
            for (int i = 0; i < 9; i++)
                row[i] = board[rowIndex, i];
            return row;
        }

        // Get a column in an array
        private static int[] GetColumn(int[,] board, int colIndex)
        {
            int[] column = new int[9];
            for (int i = 0; i < 9; i++)
                column[i] = board[i, colIndex];
            return column;
        }

        // Get a 3x3 grid
        private static int[] GetBox(int[,] board, int startRow, int startCol)
        {
            int[] box = new int[9];
            int index = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    box[index++] = board[startRow + i, startCol + j];
                }
            }
            return box;
        }
    }
}
