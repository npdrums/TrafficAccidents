using Application.Services;

using Domain.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class BusinessServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services) 
        => services.AddTransient<ITrafficAccidentsService, TrafficAccidentsService>();
}