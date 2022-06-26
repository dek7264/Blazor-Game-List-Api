namespace DatabaseContext.Internal
{
    public abstract class DatabaseCommandBuilderWrapper
    {
        public abstract (string sql, object parameters) Build(DatabaseObjectFactory databaseObjectFactory, IDatabaseCommand command);
        public abstract (string sql, object parameters) Build(DatabaseObjectFactory databaseObjectFactory, IDatabaseCommand command, string parameterSuffix);
    }

    public class DatabaseCommandBuilderWrapper<TCommand> : DatabaseCommandBuilderWrapper
        where TCommand : IDatabaseCommand
    {
        public override (string sql, object parameters) Build(DatabaseObjectFactory databaseObjectFactory, IDatabaseCommand command)
        {
            var builder = databaseObjectFactory.Create<IDatabaseCommandBuilder<TCommand>>();
            return builder.Build((TCommand)command);
        }

        public override (string sql, object parameters) Build(DatabaseObjectFactory databaseObjectFactory, IDatabaseCommand command, string parameterSuffix)
        {
            var builder = databaseObjectFactory.Create<IDatabaseCommandBuilder<TCommand>>();
            return builder.Build((TCommand)command, parameterSuffix);
        }
    }
}