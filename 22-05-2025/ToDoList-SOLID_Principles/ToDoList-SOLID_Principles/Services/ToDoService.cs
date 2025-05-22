using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList_SOLID_Principles.Interfaces;
using ToDoList_SOLID_Principles.Models;

namespace ToDoList_SOLID_Principles.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IRepository<ToDoItem> _repository;

        public ToDoService(IRepository<ToDoItem> repository)
        {
            _repository = repository;
        }

        public void AddTask(string description)
        {
            var task = new ToDoItem { Description = description };
            _repository.Add(task);
        }

        public List<ToDoItem> ListTasks()
        {
            return _repository.GetAll();
        }
    }
}
