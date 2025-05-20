using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class EmployeeDataCollector
    {
        public static List<Employee> CollectEmployees()
        {
            List<Employee> employees = new List<Employee>();

            Console.Write("Enter number of employees: ");
            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
            {
                Console.WriteLine("Invalid number.");
                return employees;
            }

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"\nEntering details for Employee {i + 1}");
                Employee emp = new Employee();
                emp.TakeEmployeeDetailsFromUser();
                employees.Add(emp);
            }

            return employees;
        }
    }
}
