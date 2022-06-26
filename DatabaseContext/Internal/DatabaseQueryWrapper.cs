using System.Data;

namespace DatabaseContext.Internal
{
    public abstract class DatabaseQueryWrapper<TResponse>
    {
        public abstract Task<TResponse> Execute(DatabaseObjectFactory databaseObjectFactory, IDbConnection connection, IDbTransaction transaction, IDatabaseQuery<TResponse> query, CancellationToken cancellationToken);
    }

    public class DatabaseQueryWrapper<TRequest, TResponse> : DatabaseQueryWrapper<TResponse>
        where TRequest : IDatabaseQuery<TResponse>
    {

        public override Task<TResponse> Execute(DatabaseObjectFactory databaseObjectFactory, IDbConnection connection, IDbTransaction transaction, IDatabaseQuery<TResponse> query,
            CancellationToken cancellationToken)
        {
            var queryHandler = databaseObjectFactory.Create<IDatabaseQueryHandler<TRequest, TResponse>>();
            return queryHandler.Execute(connection, transaction, (TRequest)query, cancellationToken);
        }
    }
}