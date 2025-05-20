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
            // T A S K   1
            // thi is our data structure (key-value pair)
            Dictionary<int, Employee> employees = new Dictionary<int, Employee>();

            while (true)
            {
                Console.WriteLine("Enter details for a new employee:");

                Employee emp = new Employee();
                emp.TakeEmployeeDetailsFromUser();

                if (employees.ContainsKey(emp.Id))
                {
                    Console.WriteLine($"Employee with ID {emp.Id} already exists. Try again.");
                }
                else
                {
                    employees.Add(emp.Id, emp);
                }

                Console.WriteLine("Add another employee? (y/n)");
                if (Console.ReadLine()?.ToLower() != "y")
                    break;
            }

            // T A S K    2

            // Convert dictionary values to a list
            List<Employee> employeeList = employees.Values.ToList();

            // since we used Icomparable and compareTo salary in Employee, sort() sorts based on salary
            employeeList.Sort();

            Console.WriteLine("--- Employees sorted by salary ---");
            foreach (var emp in employeeList)
            {
                Console.WriteLine(emp);
            }

            Console.WriteLine("\nEnter employee ID to search:");
            if (int.TryParse(Console.ReadLine(), out int searchId))
            {
                Employee? foundEmployee = employeeList.FirstOrDefault(e => e.Id == searchId);

                if (foundEmployee != null)
                {
                    Console.WriteLine("Employee found:");
                    Console.WriteLine(foundEmployee);
                }
                else
                {
                    Console.WriteLine($"No employee found with ID {searchId}");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID input.");
            }


            // T A S K   3
            // Search employees with their name
            Console.Write("\nEnter employee name to search: ");
            string? searchName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchName))
            {
                Console.WriteLine("Invalid name.");
                return;
            }

            var matched = employeeList
                .Where(e => e.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matched.Count == 0)
            {
                Console.WriteLine($"No employees found with name \"{searchName}\".");
            }
            else
            {
                Console.WriteLine($"\nEmployees with name \"{searchName}\":");
                foreach (var emp in matched)
                {
                    Console.WriteLine(emp);
                    Console.WriteLine();
                }
            }



            // T A S K   5
            // employees older than input employee

            // find input employee's age first
            Console.Write("Enter name of the employee to compare age: ");
            string? baseName = Console.ReadLine();

            Employee? baseEmployee = employeeList
                .FirstOrDefault(e => e.Name.Equals(baseName, StringComparison.OrdinalIgnoreCase));

            // now compare input employee (baseEmployee age)
            if (baseEmployee == null)
            {
                Console.WriteLine($"Employee with name \"{baseName}\" not found.");
            }
            else
            {
                var olderEmployees = employeeList
                    .Where(e => e.Age > baseEmployee.Age)
                    .ToList();

                if (olderEmployees.Count == 0)
                {
                    Console.WriteLine($"No employees are older than {baseEmployee.Name}.");
                }
                else
                {
                    Console.WriteLine($"Employees older than {baseEmployee.Name}:");
                    foreach (var emp in olderEmployees)
                    {
                        Console.WriteLine($"- {emp.Name} (Age: {emp.Age})");
                    }
                }
            }

        }
    }
}
