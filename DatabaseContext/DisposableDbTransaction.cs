using System.Data;

namespace DatabaseContext
{
    public class DisposableDbTransaction : IDisposable
    {
        public IDbTransaction Transaction { get; }
        private Action OnDispose { get; }

        public DisposableDbTransaction(IDbTransaction transaction, Action onDispose)
        {
            Transaction = transaction;
            OnDispose = onDispose;
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }
    }
}