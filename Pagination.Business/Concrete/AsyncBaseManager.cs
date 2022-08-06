using AutoMapper;
using Core.DataAccess;
using Core.Dto;
using Core.Entity;
using Core.Utilities.Results;
using Pagination.Business.Abstract;
using Pagination.Business.Constants;

namespace Pagination.Business.Concrete
{
    public class AsyncBaseManager<TEntity, TDto> : IAsyncBaseService<TEntity, TDto>
        where TEntity : class, IEntity, new()
        where TDto : class, IDto, new()
    {
        protected readonly IAsyncEntityRepository<TEntity> Repository;
        protected readonly IMapper Mapper;
        public AsyncBaseManager(IAsyncEntityRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }
        public virtual async Task<IDataResult<TDto>> AddAsync(TDto dto)
        {
            TEntity addedEntity = Mapper.Map<TEntity>(dto);
            await Repository.AddAsync(addedEntity);
            return new SuccessDataResult<TDto>(dto, BusinessMessages.SuccessAdd);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            TEntity deletedEntity = await Repository.GetAsync(t => t.Id == id);
            await Repository.DeleteAsync(deletedEntity);
            return new SuccessResult(BusinessMessages.SuccessDelete);
        }

        public async Task<IDataResult<TDto>> GetByIdAsync(int id)
        {
            TEntity entity = await Repository.GetAsync(t => t.Id == id);
            if (entity == null)
                return new ErrorDataResult<TDto>(BusinessMessages.UnSuccessGet);
            TDto returnEntity = Mapper.Map<TDto>(entity);
            return new SuccessDataResult<TDto>(returnEntity, BusinessMessages.SuccessGet);
        }

        public async Task<IDataResult<List<TDto>>> GetListAsync()
        {
            IEnumerable<TEntity> entities = await Repository.GetListAsync();
            IEnumerable<TDto> result = Mapper.Map<IEnumerable<TDto>>(entities);
            if (result.Count() == 0)
                return new ErrorDataResult<List<TDto>>(BusinessMessages.UnSuccessList);
            return new SuccessDataResult<List<TDto>>(result.ToList(), BusinessMessages.SuccessList);
        }

        public virtual async Task<IDataResult<TDto>> UpdateAsync(int id, TDto dto)
        {
            TEntity updatedEntity = await Repository.GetAsync(t => t.Id == id);
            if (updatedEntity == null)
                return new ErrorDataResult<TDto>(BusinessMessages.UnSuccessGet);
            Mapper.Map(updatedEntity, dto);
            await Repository.UpdateAsync(updatedEntity);
            return new SuccessDataResult<TDto>(dto, BusinessMessages.SuccessUpdate);
        }
    }
}
