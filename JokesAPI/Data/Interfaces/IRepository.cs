using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JokesAPI.Data.Repositories;
using JokesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JokesAPI.Data.Interfaces
{
    /// <summary>
    /// Use as the base repository interface for each Model's Repository Interface.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        AppDbContext DataBaseContext { get; }

        /// <summary>
        /// Get a single item from the repository by Id
        /// </summary>
        /// <param name="id">Id of the item in the repository/table</param>
        /// <returns>TEntity</returns>
        Task<TEntity> GetItemAsync(long id);

        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Asynchronously Get all items in the respository for the given type TEntity
        /// </summary>
        /// <returns>IEnumerable of TEntity</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Find an item in the repository
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        RepositoryStatus Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        void SetState(TEntity entity, EntityState modified);

        Task<int> Commit(); 
    }
}
