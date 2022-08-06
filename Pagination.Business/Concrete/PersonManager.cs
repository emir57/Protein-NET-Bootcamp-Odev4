using AutoMapper;
using Core.Aspect.Autofac.Validation;
using Core.CrossCuttingConcerns.Caching;
using Core.Entity.Concrete;
using Core.Utilities.Results;
using Pagination.Business.Abstract;
using Pagination.Business.Helpers;
using Pagination.Business.Validators.FluentValidation;
using Pagination.DataAccess.Abstract;
using Pagination.Dto.Concrete;
using Pagination.Entity.Concrete;

namespace Pagination.Business.Concrete
{
    public class PersonManager : AsyncBaseManager<Person, PersonDto>, IPersonService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IPersonDal _personDal;
        public PersonManager(IPersonDal repository, IMapper mapper, ICacheManager cacheManager) : base(repository, mapper)
        {
            _cacheManager = cacheManager;
            _personDal = repository;
        }

        public async Task<PaginatedResult<IEnumerable<PersonDto>>> GetPaginationAsync(PaginationFilter paginationFilter)
        {
            string cacheKey = $"{paginationFilter.CacheKey}+{paginationFilter.PageSize}+{paginationFilter.PageNumber}";
            if (_cacheManager.IsAdd(cacheKey))
            {
                IEnumerable<PersonDto> cachePersons = _cacheManager.Get<IEnumerable<PersonDto>>(cacheKey);
                return PaginationHelper.CreatePaginatedResponse(Mapper.Map<IEnumerable<PersonDto>>(cachePersons), paginationFilter, cachePersons.Count(), skip: false);
            }
            IEnumerable<Person> persons = await _personDal.GetListAsync(paginationFilter.PageSize, (paginationFilter.PageNumber == 1 ? 0 : paginationFilter.PageNumber) * paginationFilter.PageSize);
            var result = PaginationHelper.CreatePaginatedResponse(Mapper.Map<IEnumerable<PersonDto>>(persons), paginationFilter, persons.Count(), skip: false);
            _cacheManager.Add(cacheKey, result.Data, 10);
            return result;
        }
        [ValidationAspect(typeof(PersonValidator))]
        public override Task<IDataResult<PersonDto>> AddAsync(PersonDto dto)
        {
            return base.AddAsync(dto);
        }
        [ValidationAspect(typeof(PersonValidator))]
        public override Task<IDataResult<PersonDto>> UpdateAsync(int id, PersonDto dto)
        {
            return base.UpdateAsync(id, dto);
        }

    }
}
