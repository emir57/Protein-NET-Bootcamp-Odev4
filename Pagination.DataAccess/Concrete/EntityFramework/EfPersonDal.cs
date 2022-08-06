using Core.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Pagination.DataAccess.Abstract;
using Pagination.DataAccess.Contexts;
using Pagination.Entity.Concrete;

namespace Pagination.DataAccess.Concrete.EntityFramework
{
    public class EfPersonDal : EfBaseRepository<Person, EfPaginationDbContext>, IPersonDal
    {
        public async Task<IEnumerable<Person>> GetListAsync(int? limit = 0, int? offset = 0)
        {
            return await Task.Run(() =>
            {
                using (var context = new EfPaginationDbContext())
                {
                    return context.Set<Person>().AsNoTracking()
                    .Skip((int)offset)
                    .Take((int)limit)
                    .AsEnumerable();
                }
            });
        }
    }
}
