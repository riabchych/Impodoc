using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ImpoDoc.Data
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        void Create(TEntity item);
        TEntity FindById(int id);
        Task<List<TEntity>> GetAsync();
        void Remove(TEntity item);
        void Update(TEntity item);
        List<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        List<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
