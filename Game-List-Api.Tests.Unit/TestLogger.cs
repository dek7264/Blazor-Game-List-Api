using Microsoft.Extensions.Logging;

namespace Game_List_Api.Tests.Unit
{
    public abstract class TestLogger<T> : ILogger<T>
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Log(logLevel, exception, formatter(state, exception));
        }

        public abstract void Log(LogLevel logLevel, Exception exception, string message);

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}
