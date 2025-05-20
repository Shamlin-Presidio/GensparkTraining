using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class EmployeePromotion
    {
        public static void Run()
        {
            List<string> promotionList = new List<string>();
            Console.WriteLine("Please enter the employee names in the order of their eligibility for promotion (Enter blank to stop):");

            while (true)
            {
                string? name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                promotionList.Add(name);
            }



            Console.WriteLine("\n--- Promotion List ---");
            for (int i = 0; i < promotionList.Count; i++)
            {
                Console.WriteLine($"Position {i + 1}: {promotionList[i]}");
            }


            // T A S K    2

            Console.Write("\nPlease enter the name of the employee to check promotion position: ");
            string? searchName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                int position = promotionList.IndexOf(searchName);
                if (position >= 0)
                {
                    Console.WriteLine($"\"{searchName}\" is at position {position + 1} for promotion.");
                }
                else
                {
                    Console.WriteLine($"\"{searchName}\" is not found in the promotion list.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }

            // T A S K   3

            Console.WriteLine($"\nThe current size (capacity) of the collection is: {promotionList.Capacity}");
            promotionList.TrimExcess(); // trims unused memory
            Console.WriteLine($"The size after removing the extra space is: {promotionList.Capacity}");


            // T A S K   4

            // Sor the list alphabetically
            promotionList.Sort();

            Console.WriteLine("\nPromoted employee list (sorted):");
            foreach (string emp in promotionList)
            {
                Console.WriteLine(emp);
            }

        }
    }
}
