using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryMembership.Shared.Domain.Abstractions;

public static class AggregateRepositoryExtensions
{
    public static IServiceCollection AddAggregateRepositories(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IAggregateRepository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}