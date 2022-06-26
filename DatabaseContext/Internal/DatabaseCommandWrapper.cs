using System.Data;

namespace DatabaseContext.Internal
{
    public abstract class DatabaseCommandWrapper
    {
        public abstract Task Execute(DatabaseObjectFactory databaseObjectFactory, IDbConnection connection, IDbTransaction transaction, IDatabaseCommand command, CancellationToken cancellationToken);
    }

    public class DatabaseCommandWrapper<TCommand> : DatabaseCommandWrapper
        where TCommand : IDatabaseCommand
    {

        public override Task Execute(DatabaseObjectFactory databaseObjectFactory, IDbConnection connection, IDbTransaction transaction, IDatabaseCommand command,
            CancellationToken cancellationToken)
        {
            var query = databaseObjectFactory.Create<IDatabaseCommandHandler<TCommand>>();
            return query.Execute(connection, transaction, (TCommand)command, cancellationToken);
        }
    }


}