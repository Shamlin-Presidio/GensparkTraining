using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckCardioApp.Exceptions;
using CheckCardioApp.Interfaces;

namespace CheckCardioApp.Repositories
{
    public abstract class Repository<TKey, TEntity> : IRepository<TKey, TEntity>
    {
        protected readonly List<TEntity> _items = new();
        protected int _nextId = 1;

        public virtual void Add(TEntity entity)
        {
            _items.Add(entity);
        }

        public abstract void Update(TEntity entity);
        public abstract void Delete(TKey id);

        public virtual ICollection<TEntity> GetAll()
        {
            return _items;
        }

        public abstract TEntity? GetById(TKey id);
    }
}
