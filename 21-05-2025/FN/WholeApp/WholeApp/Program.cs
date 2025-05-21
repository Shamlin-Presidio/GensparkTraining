using WholeApp.Interfaces;
using WholeApp;
using WholeApp.Models;
using WholeApp.Repositories;
using WholeApp.Services;

namespace WholeApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IRepositor<int, Employee> employeeRepository = new EmployeeRepository();
            IEmployeeService employeeService = new EmployeeService(employeeRepository);
            ManageEmployee manageEmployee = new ManageEmployee(employeeService);
            manageEmployee.Start();
        }
    }
}