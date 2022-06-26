namespace DatabaseContext
{
    public interface IDatabaseCommandBuilder<in TRequest>
        where TRequest : IDatabaseCommand
    {
        (string sql, object parameters) Build(TRequest command, string parameterSuffix = null);
    }
}