using System.Data;
using System.Data.Common;
using DatabaseContext.Internal;

namespace DatabaseContext
{
    public interface IDatabaseContext<out TConnection> : IDisposable
        where TConnection : DbConnection
    {
        Task<TResponse> ExecuteQuery<TResponse>(IDatabaseQuery<TResponse> query, CancellationToken cancellationToken = default);
        Task ExecuteCommand(IDatabaseCommand command, CancellationToken cancellationToken = default);
        Task<DisposableDbTransaction> BeginTransaction();
        TConnection Connection { get; }
        Task OpenConnection();
    }

    public abstract class DatabaseContext<TConnection> : IDatabaseContext<TConnection>
        where TConnection : DbConnection
    {
        public TConnection Connection { get; }
        private DbTransaction? Transaction { get; set; }
        private DatabaseObjectFactory DatabaseObjectFactory { get; }


        protected DatabaseContext(TConnection connection, DatabaseObjectFactory databaseObjectFactory)
        {
            Connection = connection;
            DatabaseObjectFactory = databaseObjectFactory;
        }

        public async Task<DisposableDbTransaction> BeginTransaction()
        {
            OnTransactionDispose();
            await OpenConnection();
            return new DisposableDbTransaction(Transaction = await Connection.BeginTransactionAsync(), OnTransactionDispose);
        }

        private void OnTransactionDispose()
        {
            Transaction?.Dispose();
            Transaction = null;
        }

        public async Task OpenConnection()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                await Connection.OpenAsync();
            }
        }

        public Task<TResponse> ExecuteQuery<TResponse>(IDatabaseQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            var queryWrapper = (DatabaseQueryWrapper<TResponse>)Activator.CreateInstance(typeof(DatabaseQueryWrapper<,>).MakeGenericType(query.GetType(), typeof(TResponse)));
            return queryWrapper.Execute(DatabaseObjectFactory, Connection, Transaction, query, cancellationToken);
        }

        public Task ExecuteCommand(IDatabaseCommand command, CancellationToken cancellationToken = default)
        {
            var commandWrapper = (DatabaseCommandWrapper)Activator.CreateInstance(typeof(DatabaseCommandWrapper<>).MakeGenericType(command.GetType()));
            return commandWrapper.Execute(DatabaseObjectFactory, Connection, Transaction, command, cancellationToken);
        }

        public virtual void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
        }
    }
}
