using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using WriteParameter.Abstract;
using WriteParameter.Attributes;
using WriteParameter.Exceptions;

namespace WriteParameter
{
    /// <summary>
    /// Limit and Offset
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class QueryGenerate<TEntity> : IGenerate<TEntity>
        where TEntity : class
    {
        public IGenerate<TEntity> SetLimit(int limit)
        {
            _limit = limit;
            return this;
        }

        public IGenerate<TEntity> SetOffset(int offset)
        {
            _offset = offset;
            return this;
        }
    }
    /// <summary>
    /// Select Id Columns
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class QueryGenerate<TEntity> : IGenerate<TEntity>
        where TEntity : class
    {
        public virtual IGenerate<TEntity> SelectColumn<TProperty>(Expression<Func<TEntity, TProperty>> predicate)
        {
            PropertyInfo propertyInfo = (predicate.Body as MemberExpression).Member as PropertyInfo;
            _properties.Add(propertyInfo);
            return this;
        }

        public virtual IGenerate<TEntity> SelectIdColumn<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            PropertyInfo selectedIdColumn = (expression.Body as MemberExpression).Member as PropertyInfo;
            if (selectedIdColumn != null)
                _idColumn = selectedIdColumn;
            return this;
        }

        public IGenerate<TEntity> SelectIdColumn<TProperty>(string idColumn)
        {
            PropertyInfo selectedIdColumn = typeof(TEntity).GetProperties().SingleOrDefault(x => x.Name.ToUpper() == idColumn.ToUpper());
            if (selectedIdColumn != null)
                _idColumn = selectedIdColumn;
            return this;
        }
    }

    /// <summary>
    /// Select Tables
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class QueryGenerate<TEntity> : IGenerate<TEntity>
        where TEntity : class
    {
        public virtual IGenerate<TEntity> SelectTable(string tableName)
        {
            _tableName = tableName;
            return this;
        }
        public IGenerate<TEntity> SelectTable(string tableName, string schema)
        {
            _tableName = tableName;
            _schema = schema.ToLower();
            return this;
        }

        public IGenerate<TEntity> SelectSchema(string schema)
        {
            _schema = schema.ToLower();
            return this;
        }
    }

    /// <summary>
    /// Commands
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class QueryGenerate<TEntity> : IGenerate<TEntity>
        where TEntity : class
    {
        public virtual string GenerateInsertQuery()
        {
            checkTable();
            checkSchema();
            return String.Format(_cultureInfo, $"insert into {_schema}.{_tableName} {insertIntoWriteParameters()}").Replace("ı", "i");
        }

        public virtual string GenerateUpdateQuery()
        {
            checkTable();
            checkSchema();
            return String.Format(_cultureInfo, $"update {_schema}.{_tableName} {updateWriteParameters()}").Replace("ı", "i");
        }
        public virtual string GenerateDeleteQuery()
        {
            checkTable();
            checkSchema();
            string idPropertyName = getIdColumn().Replace("\"", "");
            return String.Format(_cultureInfo, $"delete from {_schema}.{_tableName} where \"{idPropertyName}\"=@{idPropertyName}").Replace("ı", "i");
        }
    }

    /// <summary>
    /// Check functions
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class QueryGenerate<TEntity> : IGenerate<TEntity>
        where TEntity : class
    {
        protected virtual void checkTable()
        {
            if (_tableName is null)
                throw new NoSelectedTableException();
        }
        protected virtual void checkSchema()
        {
            if (_schema is null)
                _schema = "dbo";//throw new NoSelectedSchemaException();
        }

        protected virtual void checkIdColumn()
        {
            if (_idColumn is null)
                throw new NotFoundIdColumnException();
        }
    }

    /// <summary>
    /// Parameters
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class QueryGenerate<TEntity> : IGenerate<TEntity>
        where TEntity : class
    {
        protected virtual string updateWriteParameters(string? previousName = "", string? lastName = "")
        {
            List<PropertyInfo> properties = getProperties();
            string idPropertyName = getIdColumn().Replace("\"", "");

            string updateQuery = String.Join(",", properties.Select(p => p.Name == idPropertyName ? "" : $"{previousName}{p.Name}{lastName}=@{p.Name}"));

            updateQuery = updateQuery.StartsWith(",") ? updateQuery.Substring(1) : updateQuery;
            updateQuery += String.Concat(" ", $"where {previousName}{idPropertyName}{lastName}=@{idPropertyName}");
            return $"set {updateQuery}";
        }
        protected virtual string insertIntoWriteParameters()
        {
            string columns = getParametersWithoutId();
            string valueColumns = getValueParametersWithoutId();
            return $"({columns}) values ({valueColumns})";
        }

        protected virtual string getParametersWithoutId(string? previousName = "", string? lastName = "")
        {
            List<PropertyInfo> properties = getProperties();
            string idPropertyName = getIdColumn();
            string parameters = String.Join(",", properties.Select(p => $"{previousName}{p.Name}{lastName}" == idPropertyName ? "" : $"{previousName}{p.Name}{lastName}"));
            parameters = parameters.StartsWith(",") ? parameters.Substring(1) : parameters;
            return parameters;
        }
        protected virtual string getValueParametersWithoutId()
        {
            List<PropertyInfo> properties = getProperties();
            string idPropertyName = getIdColumn();
            string parameters = String.Join(",", properties.Select(p => p.Name == idPropertyName ? "" : $"@{p.Name}"));
            parameters = parameters.StartsWith(",") ? parameters.Substring(1) : parameters;
            return parameters;
        }
        protected virtual string getParametersWithId(string? previousName = "", string? lastName = "")
        {
            List<PropertyInfo> properties = getProperties();
            string parameters = String.Join(",", properties.Select(p => $"{previousName}{p.Name}{lastName}"));
            parameters = parameters.StartsWith(",") ? parameters.Substring(1) : parameters;
            return parameters;
        }

        private List<PropertyInfo> getProperties()
        {
            List<PropertyInfo> returnProperties = new List<PropertyInfo>();
            List<PropertyInfo> properties = _properties.Count == 0 ? typeof(TEntity).GetProperties().ToList() : _properties;
            foreach (PropertyInfo property in properties)
            {
                Attribute ignoreColumnAttribute = property.GetCustomAttribute(typeof(IgnoreColumnAttribute));
                Attribute idColumnAttribute = property.GetCustomAttribute(typeof(IdColumnAttribute));

                if (ignoreColumnAttribute != null && idColumnAttribute != null)
                    throw new CannotBeUseSameTimeIdColumnAndIgnoreColumnException();

                if (ignoreColumnAttribute == null)
                    returnProperties.Add(property);
            }
            return returnProperties;
        }
    }

    /// <summary>
    /// Base class and query functions
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class QueryGenerate<TEntity> : IGenerate<TEntity>
        where TEntity : class
    {
        protected List<PropertyInfo> _properties;
        protected string _tableName;
        protected string _schema;
        protected PropertyInfo _idColumn;
        protected string _orderBy;
        protected CultureInfo _cultureInfo;
        protected int _limit;
        protected int _offset;

        public QueryGenerate()
        {
            _properties = new List<PropertyInfo>();
            _cultureInfo = new CultureInfo("en-US");
        }
        public QueryGenerate(string tableName) : this()
        {
            _tableName = tableName;
        }
        public QueryGenerate(IEnumerable<PropertyInfo> properties)
        {
            _properties = properties.ToList();
        }
        public QueryGenerate(PropertyInfo[] properties)
        {
            _properties = properties.ToList();
        }

        public virtual string GenerateGetAllQuery(int? limit = 0, int? offset = 0)
        {
            if (limit != 0)
                _limit = (int)limit;
            if (offset != 0)
                _offset = (int)offset;
            return getAllQuery();
        }

        public virtual string GenerateGetByIdQuery()
        {
            return generateGetByIdQuery();
        }
        public virtual string GenerateGetByIdQuery(int id)
        {
            return generateGetByIdQuery(id);
        }

        public virtual string GenerateGetByIdQuery(string id)
        {
            return generateGetByIdQuery(id);
        }

        public virtual string GenerateGetAllOrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            return GetAllOrderBy(_orderBy, expression);
        }

        public virtual string GenerateGetAllOrderByDescending<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            return GetAllOrderByDescending(_orderBy, expression);
        }

        protected virtual string GetAllOrderBy<TProperty>(string orderBy, Expression<Func<TEntity, TProperty>> expression, string? previousName = "", string? lastName = "")
        {
            PropertyInfo propertyInfo = GetProperty(expression);
            _orderBy = $"order by {previousName}{propertyInfo.Name}{lastName}";
            return getAllQuery(_orderBy);
        }
        protected virtual string GetAllOrderByDescending<TProperty>(string orderBy, Expression<Func<TEntity, TProperty>> expression, string? previousName = "", string? lastName = "")
        {
            PropertyInfo propertyInfo = GetProperty(expression);
            _orderBy = $"order by {previousName}{propertyInfo.Name}{lastName} desc";
            return getAllQuery(_orderBy);
        }

        protected virtual string getAllQuery(string orderBy = "")
        {
            checkTable();
            checkSchema();
            string parameters = getParametersWithId();
            string pagination = _limit != 0 ? $"LIMIT {_limit} OFFSET {_offset}" : "";
            string query = String.Format(_cultureInfo, $"select {parameters} from {_schema}.{_tableName} {orderBy} {pagination}");
            return query;
        }

        protected virtual string generateGetByIdQuery(object id = null)
        {
            checkTable();
            checkSchema();
            string parameters = getParametersWithId();
            string idColumn = getIdColumn();
            string whereQuery = id == null ? $"{idColumn}=@{idColumn}" : $"{idColumn}={id}";
            string query = String.Format(_cultureInfo, $"select {parameters} from {_schema}.{_tableName} where {whereQuery}");
            return query;
        }

        protected virtual string getIdColumn(string? previousName = "", string? lastName = "")
        {
            if (_idColumn != null)
                return _idColumn.Name;

            var properties = typeof(TEntity).GetProperties().ToList();

            byte idAttributeCount = 0;
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(IdColumnAttribute));
                if (attribute is IdColumnAttribute)
                {
                    idAttributeCount++;
                    _idColumn = property;
                }
            }
            if (idAttributeCount > 1)
                throw new MoreThanOneIdColumnException();

            _idColumn = _idColumn == null ? properties.FirstOrDefault(p => p.Name.ToUpper() == "ID") : _idColumn;

            checkIdColumn();
            return $"{previousName}{_idColumn.Name}{lastName}";
        }

        protected virtual PropertyInfo GetProperty<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            PropertyInfo propertyInfo = (expression.Body as MemberExpression).Member as PropertyInfo;
            return propertyInfo;
        }
    }

}
