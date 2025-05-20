using System;
using tasks.Properties;

using System;
using tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("Choose a task to run (1 to 5):");
        Console.WriteLine("1: Instagram");
        
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
                Task1_InstagramPosts.Run();
                break;




            default:
                Console.WriteLine("Invalid choice. Please select between 1 and 5.");
                break;
        }
    }
}
