using System.Reflection;
using DatabaseContext;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseQueryServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseContextHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.Scan(x => x.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(typeof(IDatabaseQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IDatabaseCommandHandler<>)))
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IDatabaseCommandBuilder<>)))
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
            );
            services.TryAddScoped<IDatabaseCommandBuilderExecutor, DatabaseCommandBuilderExecutor>();
            services.TryAddScoped<DatabaseObjectFactory>(provider => provider.GetService);
            return services;
        }
    }
}
