using System.Reflection;
using DatabaseContext.Postgres;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseQueryServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseContext<TDatabaseContextInterface, TDatabaseContext>(this IServiceCollection services, params Assembly[] assemblies)
            where TDatabaseContextInterface : class, IPostgresDatabaseContext
            where TDatabaseContext : PostgresDatabaseContext, TDatabaseContextInterface
        {
            return services.AddDatabaseContextHandlers(assemblies)
                .AddScoped<TDatabaseContextInterface, TDatabaseContext>();
        }
    }
}
