using Microsoft.Extensions.DependencyInjection;

namespace DSK.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApplication(
        this IServiceCollection services)
    {

        AddMappings(services);

        return services;
    }

    private static void AddMappings(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }
}