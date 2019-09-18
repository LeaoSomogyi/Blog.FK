using Blog.FK.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.FK.Infra.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        #region "  Properties  "

        protected DbContext DbContext;

        #endregion

        #region "  Constructors  "

        public BaseRepository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        #endregion

        #region "  IBaseRepository<TEntity>  "

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                var obj = await DbContext.Set<TEntity>().AddAsync(entity);

                return obj.Entity;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TEntity> FindAsync(Guid id)
        {
            try
            {
                return await DbContext.Set<TEntity>().FindAsync(id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await DbContext.Set<TEntity>().ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public void Remove(TEntity entity)
        {
            try
            {
                DbContext.Set<TEntity>().Remove(entity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return await DbContext.Set<TEntity>().Where(expression).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public void Update(TEntity entity)
        {
            try
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region "  IDisposable  "

        public void Dispose()
        {
            DbContext.SaveChanges();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
