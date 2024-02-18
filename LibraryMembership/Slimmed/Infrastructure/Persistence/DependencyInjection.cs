using LibraryMembership.Slimmed.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<LibraryMembershipContext>(opts =>
        {
            opts.UseInMemoryDatabase("LibraryMembership");
        });
        
        services.AddScoped<ILibraryMembershipContext, LibraryMembershipContextWrapper>();
        
        services.AddScoped<ISaveChangesInterceptor, PublishDomainEventsInterceptor>();
        
        return services;
    }
}

