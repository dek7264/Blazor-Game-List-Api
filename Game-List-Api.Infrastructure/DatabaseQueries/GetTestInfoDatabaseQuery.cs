using Dapper;
using Npgsql;

namespace Game_List_Api.Infrastructure.DatabaseQueries
{
    public class GetTestInfoDatabaseQuery
    {
        private IDatabaseConnectionSettings DbSettings { get; }
        public GetTestInfoDatabaseQuery(IDatabaseConnectionSettings settings)
        {
            DbSettings = settings;
        }

        public async Task<IReadOnlyList<int>> GetTestInfo()
        {
            using var connection = new NpgsqlConnection(DbSettings.ConnectionString);
            return (await connection.QueryAsync<int>("select * from test")).ToList();
        }
    }
}
