using ToDoList_SOLID_Principles.Interfaces;
using ToDoList_SOLID_Principles.Repositories;
using ToDoList_SOLID_Principles.Services;

class Program
{
    static void Main()
    {
        IToDoService service = new ToDoService(new InMemoryToDoRepository());

        while (true)
        {
            Console.WriteLine("\n1. Add a Task and get it done!");
            Console.WriteLine("2. Show my tasks");
            Console.WriteLine("3. Nah, I wanna exit");
            Console.Write("Select an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter task description: ");
                    var desc = Console.ReadLine();
                    service.AddTask(desc!);
                    Console.WriteLine("Task added.");
                    break;

                case "2":
                    var tasks = service.ListTasks();
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine("No tasks found.");
                    }
                    else
                    {
                        foreach (var t in tasks)
                        {
                            Console.WriteLine($"[{t.Id}] {t.Description}");
                        }
                    }
                    break;

                case "3":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}