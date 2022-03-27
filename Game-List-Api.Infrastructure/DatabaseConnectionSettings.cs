using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Game_List_Api.Infrastructure
{
    public interface IDatabaseConnectionSettings
    {
        string ConnectionString { get; }
    }

    public class DatabaseConnectionSettings : IDatabaseConnectionSettings
    {
        public string ConnectionString { get; set; }

        public DatabaseConnectionSettings(IConfiguration configuration)
        {
            var databaseUrl = configuration["DATABASE_URL"] ?? throw new InvalidOperationException("DATABASE_URL environment variable not found!");
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/')
            };

            ConnectionString = builder.ToString();
        }
    }
}