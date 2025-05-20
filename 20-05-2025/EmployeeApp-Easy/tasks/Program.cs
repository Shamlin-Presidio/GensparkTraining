using System;
using tasks;

class Program
{
    static void Main()
    {
        // Step 1: Collect employee data
        List<Employee> allEmployees = EmployeeDataCollector.CollectEmployees();

        Console.WriteLine("\n--- All Employees ---");
        foreach (Employee emp in allEmployees)
        {
            Console.WriteLine(emp);
            Console.WriteLine();
        }

        // Step 2: Promotion order
        EmployeePromotion.Run();
    }
}
