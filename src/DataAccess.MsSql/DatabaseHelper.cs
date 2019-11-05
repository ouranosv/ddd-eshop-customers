using Microsoft.Extensions.Configuration;

namespace DataAccess.MsSql
{
    public static class DatabaseHelper
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var dbHost = configuration["DatabaseHost"];
            var dbName = configuration["DatabaseName"];
            var dbUser = configuration["DatabaseUser"];
            var dbPassword = configuration["DatabasePassword"];

            return $"User ID={dbUser};Password={dbPassword};Server={dbHost};Database={dbName};Integrated Security=true;Pooling=true;";
        }
    }
}
