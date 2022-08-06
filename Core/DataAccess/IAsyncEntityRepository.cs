using Core.Dto;
using Core.Entity;
using System.Linq.Expressions;

namespace Core.DataAccess
{
    public interface IAsyncEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetListAsync();
        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
