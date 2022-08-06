using System.Linq.Expressions;
using System.Reflection;
using WriteParameter.Abstract;

namespace WriteParameter
{
    public class NpgQueryGenerate<TEntity> : QueryGenerate<TEntity>
        where TEntity : class
    {
        public NpgQueryGenerate()
        {
        }

        public NpgQueryGenerate(string tableName) : base(tableName)
        {
        }

        public NpgQueryGenerate(IEnumerable<PropertyInfo> properties) : base(properties)
        {
        }

        public NpgQueryGenerate(PropertyInfo[] properties) : base(properties)
        {
        }

        protected override string getParametersWithId(string? previousName = "", string? lastName = "")
        {
            return base.getParametersWithId("\"", "\"");
        }
        protected override string getParametersWithoutId(string? previousName = "", string? lastName = "")
        {
            return base.getParametersWithoutId("\"", "\"");
        }
        protected override string GetAllOrderBy<TProperty>(string orderBy, Expression<Func<TEntity, TProperty>> expression, string? previousName = "", string? lastName = "")
        {
            return base.GetAllOrderBy(orderBy, expression, "\"", "\"");
        }
        protected override string GetAllOrderByDescending<TProperty>(string orderBy, Expression<Func<TEntity, TProperty>> expression, string? previousName = "", string? lastName = "")
        {
            return base.GetAllOrderByDescending(orderBy, expression, "\"", "\"");
        }
        protected override string getIdColumn(string? previousName = "", string? lastName = "")
        {
            return base.getIdColumn("\"", "\"");
        }
        protected override string updateWriteParameters(string? previousName = "", string? lastName = "")
        {
            return base.updateWriteParameters("\"", "\"");
        }
        public override IGenerate<TEntity> SelectTable(string tableName)
        {
            return base.SelectTable($"\"{tableName}\"");
        }
    }
}
