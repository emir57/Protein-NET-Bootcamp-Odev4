using Core.DataAccess;
using Pagination.Entity.Concrete;

namespace Pagination.DataAccess.Abstract
{
    public interface IPersonDal : IAsyncEntityRepository<Person>
    {
        Task<IEnumerable<Person>> GetListAsync(int? limit = 0, int? offset = 0);
    }
}
