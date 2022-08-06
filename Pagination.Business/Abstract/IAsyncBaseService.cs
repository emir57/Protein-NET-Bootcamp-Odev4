using Core.Dto;
using Core.Entity;
using Core.Utilities.Results;

namespace Pagination.Business.Abstract
{
    public interface IAsyncBaseService<TEntity, TDto>
        where TEntity : class, IEntity, new()
        where TDto : class, IDto, new()
    {
        Task<IDataResult<TDto>> AddAsync(TDto dto);
        Task<IDataResult<TDto>> UpdateAsync(int id, TDto dto);
        Task<IResult> DeleteAsync(int id);
        Task<IDataResult<TDto>> GetByIdAsync(int id);
        Task<IDataResult<List<TDto>>> GetListAsync();
    }
}
