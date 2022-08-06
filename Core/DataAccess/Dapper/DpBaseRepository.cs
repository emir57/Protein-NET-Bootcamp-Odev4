using Core.Entity;
using Dapper;
using System.Data;
using System.Linq.Expressions;
using WriteParameter;

namespace Core.DataAccess.Dapper
{
    public class DpBaseRepository<TEntity, TContext> : IAsyncEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : class, IContext, new()
    {
        private readonly string _schema;
        private readonly string _tableName;
        public DpBaseRepository(string schema, string tableName)
        {
            _schema = schema;
            _tableName = tableName;
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using (var conn = new TContext().CreateConnection())
            {
                string query = new NpgQueryGenerate<TEntity>()
                    .SelectSchema(_schema).SelectTable(_tableName)
                    .GenerateInsertQuery();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                await conn.ExecuteAsync(query, entity);
                return entity;
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            using (var conn = new TContext().CreateConnection())
            {
                string query = new NpgQueryGenerate<TEntity>()
                    .SelectSchema(_schema).SelectTable(_tableName)
                    .GenerateDeleteQuery();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                await conn.ExecuteAsync(query, entity);
            }
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var conn = new TContext().CreateConnection())
            {
                string query = new NpgQueryGenerate<TEntity>()
                    .SelectSchema(_schema).SelectTable(_tableName)
                    .GenerateGetAllQuery();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var result = await conn.QueryAsync<TEntity>(query);
                return result.AsQueryable().SingleOrDefault(predicate);
            }
        }

        public async Task<IEnumerable<TEntity>> GetListAsync()
        {
            using (var conn = new TContext().CreateConnection())
            {
                string query = new NpgQueryGenerate<TEntity>()
                    .SelectSchema(_schema).SelectTable(_tableName)
                    .GenerateGetAllQuery();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var result = await conn.QueryAsync<TEntity>(query);
                return result;
            }
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var conn = new TContext().CreateConnection())
            {
                string query = new NpgQueryGenerate<TEntity>()
                    .SelectSchema(_schema).SelectTable(_tableName)
                    .GenerateGetAllQuery();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var result = await conn.QueryAsync<TEntity>(query);
                return result.AsQueryable().Where(predicate);
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            using (var conn = new TContext().CreateConnection())
            {
                string query = new NpgQueryGenerate<TEntity>()
                    .SelectSchema(_schema).SelectTable(_tableName)
                    .GenerateUpdateQuery();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                await conn.ExecuteAsync(query, entity);
                return entity;
            }
        }
    }
}
