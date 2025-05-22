using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList_SOLID_Principles.Interfaces;
using ToDoList_SOLID_Principles.Models;

namespace ToDoList_SOLID_Principles.Repositories
{
    public class InMemoryToDoRepository : IRepository<ToDoItem>
    {
        private readonly List<ToDoItem> _items = new();
        private int _nextId = 1;

        public void Add(ToDoItem item)
        {
            item.Id = _nextId++;
            _items.Add(item);
        }

        public List<ToDoItem> GetAll()
        {
            return _items;
        }
    }
}
