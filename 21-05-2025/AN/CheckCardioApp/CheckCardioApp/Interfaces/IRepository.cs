using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckCardioApp.Interfaces
{
    public interface IRepository<TKey, TEntity>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TKey id);
        ICollection<TEntity> GetAll();
        TEntity? GetById(TKey id);

    }
}
