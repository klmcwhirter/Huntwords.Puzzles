#pragma warning disable CS1572, CS1573, CS1591
using System.Linq;

namespace puzzles.Repositories
{
    public interface IRepository<TKey, TEntity>
    {
        TEntity Get(TKey id);

        IQueryable<TEntity> GetAll();

        TEntity Add(TEntity entity);

        TEntity Update(TKey id, TEntity entity);

        void Delete(TKey id);

        void SaveChanges();
    }
}
