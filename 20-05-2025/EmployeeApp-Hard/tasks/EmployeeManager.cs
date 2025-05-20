using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class EmployeeManager
    {
        static Dictionary<int, Employee> employees = new Dictionary<int, Employee>();

        public static void Run()
        {
            while (true)
            {
                Console.WriteLine("\n--- Employee Management Menu ---");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Display All Employees");
                Console.WriteLine("3. Modify Employee (by ID)");
                Console.WriteLine("4. View Employee (by ID)");
                Console.WriteLine("5. Delete Employee (by ID)");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddEmployee();
                        break;
                    case "2":
                        DisplayAll();
                        break;
                    case "3":
                        ModifyEmployee();
                        break;
                    case "4":
                        ViewEmployee();
                        break;
                    case "5":
                        DeleteEmployee();
                        break;
                    case "6":
                        Console.WriteLine("Exiting application.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void AddEmployee()
        {
            Console.WriteLine("\nEnter Employee ID:");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            if (employees.ContainsKey(id))
            {
                Console.WriteLine("Employee with this ID already exists.");
                return;
            }

            Employee emp = new Employee { Id = id };
            emp.TakeEmployeeDetailsFromUser();
            employees[id] = emp;

            Console.WriteLine("Employee added successfully.");
        }

        static void DisplayAll()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees to display.");
                return;
            }

            Console.WriteLine("\n--- All Employees ---");
            foreach (var emp in employees.Values)
            {
                Console.WriteLine(emp);
                Console.WriteLine("------------------");
            }
        }

        static void ModifyEmployee()
        {
            Console.WriteLine("\nEnter Employee ID to modify:");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            if (employees.TryGetValue(id, out Employee? emp))
            {
                Console.WriteLine("Enter new details and sorry, you can't modify the id :)  :");
                emp.TakeEmployeeDetailsFromUser();
                Console.WriteLine("Employee details updated.");
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }

        static void ViewEmployee()
        {
            Console.WriteLine("\nEnter Employee ID to view:");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            if (employees.TryGetValue(id, out Employee? emp))
            {
                Console.WriteLine("\n--- Employee Details ---");
                Console.WriteLine(emp);
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }

        static void DeleteEmployee()
        {
            Console.WriteLine("\nEnter Employee ID to delete:");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            if (employees.Remove(id))
            {
                Console.WriteLine("Employee deleted successfully.");
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }
    }
}
