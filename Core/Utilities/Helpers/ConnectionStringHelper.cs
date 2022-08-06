using Microsoft.Extensions.Configuration;

namespace Core.Utilities.Helpers
{
    public class ConnectionStringHelper
    {
        public static string GetConnectionString(string connectionString)
        {
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            return configurationBuilder.GetConnectionString(connectionString);
        }
    }
}
