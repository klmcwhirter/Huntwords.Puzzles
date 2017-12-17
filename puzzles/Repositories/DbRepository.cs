using System;
using System.Linq;
using puzzles.Models;
using Microsoft.EntityFrameworkCore;

namespace puzzles.Repositories
{
    /// <summary>
    /// Generic repository that uses the db for storage
    /// </summary>
    public abstract class DbRepository<K, T> : IRepository<K, T>
    where T : class
    {
        /// <summary>
        /// DbContext instance
        /// </summary>
        /// <returns></returns>
        protected DbContext DbContext { get; }
        /// <summary>
        /// DbSet operated on by this repository
        /// </summary>
        /// <returns></returns>
        protected abstract DbSet<T> DbSet { get; }

        /// <summary>
        /// Construct a DbRepository
        /// </summary>
        /// <param name="context">DbContext</param>
        public DbRepository(DbContext context)
        {
            DbContext = context;
        }

        /// <summary>
        /// Add an entity to the DbSet
        /// </summary>
        /// <param name="entity">T</param>
        /// <returns>T</returns>
        public virtual T Add(T entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        /// <summary>
        /// Delete an entity from the DbSet
        /// </summary>
        /// <param name="id">K</param>
        public virtual void Delete(K id)
        {
            var entity = Get(id);
            DbSet.Remove(entity);
        }

        /// <summary>
        /// Get an entity
        /// </summary>
        /// <param name="id">K</param>
        /// <returns>T</returns>
        public virtual T Get(K id)
        {
            var rc = DbSet.Find(id);
            return rc;
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>IQueryable&lt;T&gt;</returns>
        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="id">K</param>
        /// <param name="entity">T</param>
        /// <returns>T</returns>
        public virtual T Update(K id, T entity)
        {
            var entityEntry = DbSet.Attach(entity);
            entityEntry.State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// Save all changes being tracked by the DbContext
        /// </summary>
        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }
}
