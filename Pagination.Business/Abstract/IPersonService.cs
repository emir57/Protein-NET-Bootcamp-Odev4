using Core.Entity.Concrete;
using Core.Utilities.Results;
using Pagination.Dto.Concrete;
using Pagination.Entity.Concrete;

namespace Pagination.Business.Abstract
{
    public interface IPersonService : IAsyncBaseService<Person, PersonDto>
    {
        Task<PaginatedResult<IEnumerable<PersonDto>>> GetPaginationAsync(PaginationFilter paginationFilter);
    }
}
