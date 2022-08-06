using Core.DataAccess.Dapper;
using Dapper;
using Pagination.DataAccess.Abstract;
using Pagination.DataAccess.Contexts;
using Pagination.Entity.Concrete;
using System.Data;
using WriteParameter;

namespace Pagination.DataAccess.Concrete.Dapper
{
    public class DpPersonDal : DpBaseRepository<Person, DpPaginationDbContext>, IPersonDal
    {
        private readonly string _tablename;
        private readonly string _schema;
        public DpPersonDal(string tablename = "Person", string schema = "dbo") : base(schema, tablename)
        {
            _tablename = tablename;
            _schema = schema;
        }

        public async Task<IEnumerable<Person>> GetListAsync(int? limit = 0, int? offset = 0)
        {
            using (var conn = new DpPaginationDbContext().CreateConnection())
            {
                string query = new NpgQueryGenerate<Person>()
                    .SelectSchema(_schema).SelectTable(_tablename)
                    .GenerateGetAllQuery((int)limit, (int)offset);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var result = await conn.QueryAsync<Person>(query);
                return result;
            }
        }
    }
}
