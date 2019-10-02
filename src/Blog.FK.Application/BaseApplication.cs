using Blog.FK.Application.Interfaces;
using Blog.FK.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.FK.Application
{
    public class BaseApplication<TEntity> : IBaseApplication<TEntity> where TEntity : class
    {
        #region "  Services & Repositories  "

        private readonly IBaseRepository<TEntity> _baseRepository;

        #endregion

        #region "  Constructors  "

        public BaseApplication(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        #endregion

        #region "  IBaseApplication  "

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            return await _baseRepository.AddAsync(entity);
        }

        public void Dispose()
        {
            _baseRepository.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual async Task<TEntity> FindAsync(Guid id)
        {
            return await _baseRepository.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _baseRepository.GetAllAsync();
        }

        public virtual void Remove(TEntity entity)
        {
            if (entity != null)
            {
                _baseRepository.Remove(entity);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _baseRepository.SearchAsync(expression);
        }

        public virtual void Update(TEntity entity)
        {
            _baseRepository.Update(entity);
        }

        #endregion
    }
}
