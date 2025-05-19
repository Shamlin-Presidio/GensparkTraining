using System;
using tasks.Properties;

using System;
using tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("Choose a task to run (1 to 5):");
        Console.WriteLine("1: Greet User");
        Console.WriteLine("2: Find Largest of Two Numbers");
        Console.WriteLine("3: Perform Arithmetic Operation");
        Console.WriteLine("4: Check Credentials");
        Console.WriteLine("5: Count Numbers Divisible by 7");
        Console.WriteLine("6: Count the Frequency of Each Element");
        Console.WriteLine("7: Rotate the array to the left by one position");
        Console.WriteLine("8: Merge Two Arrays");
        Console.WriteLine("9: Guess Word Game");
        Console.WriteLine("10: Validate Sudoku");
        Console.WriteLine("11: Validate Sudoku Board");
        Console.WriteLine("12: Encrypt Decrypt");
        Console.Write("Enter your choice: ");

        string? input = Console.ReadLine();
        if (!int.TryParse(input, out int choice))
        {
            Console.WriteLine("Invalid input. Please enter a number from 1 to 5.");
            return;
        }

        switch (choice)
        {
            case 1:
                Console.Write("Enter your name: ");
                string? name = Console.ReadLine();
                Task1_GreetUser.Run(name);
                break;

            case 2:
                Console.Write("Enter the first number: ");
                double firstNumber = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter the second number: ");
                double secondNumber = Convert.ToDouble(Console.ReadLine());

                double largest = Task2_FindAndPrintLargestNumber.Run(firstNumber, secondNumber);
                Console.WriteLine($"The largest number is: {largest}");
                break;

            case 3:
                Console.Write("Enter the first number: ");
                double n1 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter the second number: ");
                double n2 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter the operator (+, -, *, /): ");
                char op = Convert.ToChar(Console.ReadLine());

                double result = Task3_PerformOperation.Run(n1, n2, op);
                Console.WriteLine($"Result: {result}");
                break;

            case 4:
                Task4_CheckCredentials.Run();
                break;

            case 5:
                int[] nums = new int[10];
                Console.WriteLine("Enter 10 numbers:");
                for (int i = 0; i < 10; i++)
                {
                    Console.Write($"Number {i + 1}: ");
                    nums[i] = Convert.ToInt32(Console.ReadLine());
                }

                int divisibleCount = Task5_NumberDivisibleBy7.Run(nums);
                Console.WriteLine($"Numbers divisible by 7: {divisibleCount}");
                break;

            case 6:
                Console.Write("Enter number of elements: ");
                int count = Convert.ToInt32(Console.ReadLine());

                int[] inputArray = new int[count];
                Console.WriteLine("Enter the elements:");

                for (int i = 0; i < count; i++)
                {
                    Console.Write($"Element {i + 1}: ");
                    inputArray[i] = Convert.ToInt32(Console.ReadLine());
                }

                Task6_ElementFrequency.Run(inputArray);
                break;


            case 7:
                Console.Write("Enter number of elements: ");
                int len = Convert.ToInt32(Console.ReadLine());
                int[] rotateArray = new int[len];

                Console.WriteLine("Enter the elements:");
                for (int i = 0; i < len; i++)
                {
                    Console.Write($"Element {i + 1}: ");
                    rotateArray[i] = Convert.ToInt32(Console.ReadLine());
                }

                Task7_ArrayLeftRotate.Run(rotateArray);
                break;

            case 8:
                Console.Write("Enter number of elements in first array: ");
                int size1 = Convert.ToInt32(Console.ReadLine());
                int[] arr1 = new int[size1];

                Console.WriteLine("Enter elements of first array:");
                for (int i = 0; i < size1; i++)
                {
                    Console.Write($"Element {i + 1}: ");
                    arr1[i] = Convert.ToInt32(Console.ReadLine());
                }

                Console.Write("Enter number of elements in second array: ");
                int size2 = Convert.ToInt32(Console.ReadLine());
                int[] arr2 = new int[size2];

                Console.WriteLine("Enter elements of second array:");
                for (int i = 0; i < size2; i++)
                {
                    Console.Write($"Element {i + 1}: ");
                    arr2[i] = Convert.ToInt32(Console.ReadLine());
                }

                Task8_MergeArrays.Run(arr1, arr2);
                break;

            case 9:
                Task9_BullsAndCows.Run();
                break;

            case 10:
                Task10_ValidateSudokuRow.Run();
                break;

            case 11:
                Task11_ValidateSudokuBoard.Run();
                break;

            case 12:
                Task12_ShiftCipher.Run();
                break;



            default:
                Console.WriteLine("Invalid choice. Please select between 1 and 5.");
                break;
        }
    }
}
