using System.Data;

namespace DatabaseContext
{
    public interface IDatabaseCommandHandler<in TRequest>
        where TRequest : IDatabaseCommand
    {
        Task Execute(IDbConnection connection, IDbTransaction transaction, TRequest command, CancellationToken cancellationToken);
    }
}