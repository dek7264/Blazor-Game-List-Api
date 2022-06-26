using Npgsql;

namespace DatabaseContext.Postgres
{
    public interface IPostgresDatabaseContext : IDatabaseContext<NpgsqlConnection>
    { }

    public abstract class PostgresDatabaseContext : DatabaseContext<NpgsqlConnection>, IPostgresDatabaseContext
    {
        protected PostgresDatabaseContext(string connectionString, DatabaseObjectFactory databaseObjectFactory)
            : base(CreateConnection(connectionString), databaseObjectFactory)
        { }

        private static NpgsqlConnection CreateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}