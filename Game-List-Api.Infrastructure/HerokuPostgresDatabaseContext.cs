using DatabaseContext;
using DatabaseContext.Postgres;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Game_List_Api.Infrastructure
{
    public interface IHerokuPostgresDatabaseContext : IPostgresDatabaseContext
    { }

    public class HerokuPostgresDatabaseContext : PostgresDatabaseContext, IHerokuPostgresDatabaseContext
    {
        public HerokuPostgresDatabaseContext(IConfiguration configuration, DatabaseObjectFactory databaseObjectFactory)
            : base(CreateConnectionString(configuration["DATABASE_URL"]), databaseObjectFactory)
        { }

        private static string CreateConnectionString(string databaseUrl)
        {
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

            return builder.ToString();
        }
    }
}
