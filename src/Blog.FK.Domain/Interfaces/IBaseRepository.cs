using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.FK.Domain.Interfaces
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// Method to asynchronously add an entity to database
        /// </summary>
        /// <param name="entity">Entity with mapping</param>
        /// <returns>Entity added to database</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Method to asynchronously get an entity by Id
        /// </summary>
        /// <param name="id">Uniqueidentifier (PK)</param>
        /// <returns>Entity</returns>
        Task<TEntity> FindAsync(Guid id);

        /// <summary>
        /// Method to asynchronously get all records on table
        /// </summary>
        /// <returns>A list with all records</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Method to remove an entity from database
        /// </summary>
        /// <param name="entity">Entity to be removed</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Method to asynchronously search an set of records on database
        /// </summary>
        /// <param name="expression">Lambda to pass on "where" clause</param>
        /// <returns>A list with records</returns>
        Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Method to update some Entity
        /// </summary>
        /// <param name="entity">Updated Entity</param>
        void Update(TEntity entity);
    }
}
