using Dapper;
using DatabaseContext;
using System.Data;

namespace Game_List_Api.Infrastructure.DatabaseQueries
{
    public record GetTestInfoDatabaseQuery : IDatabaseQuery<IReadOnlyList<int>>;

    public class GetTestInfoDatabaseQueryHandler : IDatabaseQueryHandler<GetTestInfoDatabaseQuery, IReadOnlyList<int>>
    {
        public async Task<IReadOnlyList<int>> Execute(IDbConnection connection, IDbTransaction transaction, GetTestInfoDatabaseQuery query, CancellationToken cancellationToken)
        {
            return (await connection.QueryAsync<int>("select * from test")).ToList();
        }
    }
}
