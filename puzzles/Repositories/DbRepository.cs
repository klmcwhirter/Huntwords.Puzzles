using System;
using System.Linq;
using puzzles.Models;
using Microsoft.EntityFrameworkCore;

namespace puzzles.Repositories
{
    public abstract class DbRepository<K, T> : IRepository<K, T>
    where T : class
    {
        protected PuzzlesDbContext DbContext { get; }
        protected abstract DbSet<T> DbSet { get; }

        public DbRepository(PuzzlesDbContext context)
        {
            DbContext = context;
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(K id)
        {
            var entity = Get(id);
            DbSet.Remove(entity);
        }

        public virtual T Get(K id)
        {
            var rc = DbSet.Find(id);
            return rc;
        }

        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public virtual void Update(K id, T entity)
        {
            var entityEntry = DbSet.Attach(entity);
            entityEntry.State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }
}
