using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList_SOLID_Principles.Models;

namespace ToDoList_SOLID_Principles.Interfaces
{
    public interface IToDoService
    {
        void AddTask(string description);
        List<ToDoItem> ListTasks();
    }
}
