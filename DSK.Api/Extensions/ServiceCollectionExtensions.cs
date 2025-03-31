namespace DSK.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureWebApplication(
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