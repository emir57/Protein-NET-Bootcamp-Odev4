using System.Data;

namespace Core.DataAccess
{
    public interface IContext
    {
        IDbConnection CreateConnection();
    }
}
