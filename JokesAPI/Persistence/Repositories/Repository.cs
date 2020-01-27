using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JokesAPI.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JokesAPI.Persistence
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> dbEntity;

        public Repository(DbContext context)
        {
            Context = context;

            if (context != null)
                dbEntity = context.Set<TEntity>();
        }

        public async Task<TEntity> GetItem(long id)
        {
            // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.

            return await dbEntity.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await dbEntity.ToListAsync();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dbEntity.Where(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return dbEntity.SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            dbEntity.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbEntity.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            dbEntity.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbEntity.RemoveRange(entities);
        }
    }
}
