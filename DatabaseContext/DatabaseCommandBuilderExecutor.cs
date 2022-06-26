using DatabaseContext.Internal;

namespace DatabaseContext
{
    public interface IDatabaseCommandBuilderExecutor
    {
        (string sql, object parameters) Execute(IDatabaseCommand command);
        (string sql, object parameters) Execute(IDatabaseCommand command, string parameterSuffix);
    }

    public class DatabaseCommandBuilderExecutor : IDatabaseCommandBuilderExecutor
    {
        private DatabaseObjectFactory Factory { get; }

        public DatabaseCommandBuilderExecutor(DatabaseObjectFactory factory)
        {
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public (string sql, object parameters) Execute(IDatabaseCommand command)
        {
            var builder = (DatabaseCommandBuilderWrapper)Activator.CreateInstance(typeof(DatabaseCommandBuilderWrapper<>).MakeGenericType(command.GetType()));
            return builder.Build(Factory, command);
        }

        public (string sql, object parameters) Execute(IDatabaseCommand command, string parameterSuffix)
        {
            var builder = (DatabaseCommandBuilderWrapper)Activator.CreateInstance(typeof(DatabaseCommandBuilderWrapper<>).MakeGenericType(command.GetType()));
            return builder.Build(Factory, command, parameterSuffix);
        }
    }
}