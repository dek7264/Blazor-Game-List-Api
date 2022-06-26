using System.Data;

namespace Dapper
{
    public static class DapperExtensions
    {
        public static Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection dbConnection, string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken, CommandType? commandType = null, int? commandTimeout = null)
        {
            return dbConnection.QueryAsync<T>(new CommandDefinition(sql, param, transaction, cancellationToken: cancellationToken, commandTimeout: commandTimeout, commandType: commandType));
        }

        public static Task<int> ExecuteAsync(this IDbConnection dbConnection, string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            return dbConnection.ExecuteAsync(new CommandDefinition(sql, param, transaction, cancellationToken: cancellationToken));
        }

        public static Task<T> QuerySingleAsync<T>(this IDbConnection dbConnection, string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            return dbConnection.QuerySingleAsync<T>(new CommandDefinition(sql, param, transaction, cancellationToken: cancellationToken));
        }

        public static Task<T> QuerySingleOrDefaultAsync<T>(this IDbConnection dbConnection, string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            return dbConnection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, param, transaction, cancellationToken: cancellationToken));
        }

        public static Task<T> QueryFirstOrDefaultAsync<T>(this IDbConnection dbConnection, string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            return dbConnection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, param, transaction, cancellationToken: cancellationToken));
        }

        public static Task<SqlMapper.GridReader> QueryMultipleAsync(this IDbConnection dbConnection, string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken)
        {
            return dbConnection.QueryMultipleAsync(new CommandDefinition(sql, param, transaction, cancellationToken: cancellationToken));
        }

        public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(this IDbConnection dbConnection, string sql, Func<TFirst, TSecond, TReturn> map, object param,
            IDbTransaction transaction, string splitOn, CancellationToken cancellationToken)
        {
            return dbConnection.QueryAsync(new CommandDefinition(sql, param, transaction, cancellationToken: cancellationToken), map, splitOn);
        }

        public static async Task<TReturn> QuerySingleAsync<TFirst, TSecond, TReturn>(this IDbConnection dbConnection, string sql, Func<TFirst, TSecond, TReturn> map, object param,
            IDbTransaction transaction, string splitOn, CancellationToken cancellationToken)
        {
            return (await dbConnection.QueryAsync(new CommandDefinition(sql, param, transaction, cancellationToken: cancellationToken), map, splitOn)).Single();
        }

        public static async Task<TReturn> QuerySingleAsync<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection dbConnection, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param,
            IDbTransaction transaction, string splitOn, CancellationToken cancellationToken)
        {
            return (await dbConnection.QueryAsync(new CommandDefinition(sql, param, transaction, cancellationToken: cancellationToken), map, splitOn)).Single();
        }
    }
}
