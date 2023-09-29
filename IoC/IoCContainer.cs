using Application.DependencyInjection;

using Infrastructure.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public static class IoCContainer
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services
            .AddDatabaseServices(configuration.GetConnectionString("DefaultConnection"))
            .AddBusinessServices();

        return services;
    }
}