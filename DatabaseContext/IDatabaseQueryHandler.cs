using System.Data;

namespace DatabaseContext
{
    public interface IDatabaseQueryHandler<in TRequest, TResponse>
        where TRequest : IDatabaseQuery<TResponse>
    {
        Task<TResponse> Execute(IDbConnection connection, IDbTransaction transaction, TRequest query, CancellationToken cancellationToken);
    }
}