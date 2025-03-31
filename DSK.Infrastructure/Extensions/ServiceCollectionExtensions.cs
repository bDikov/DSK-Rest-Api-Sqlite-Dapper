using DSK.Domain.Repositories;
using DSK.Domain.Services;
using DSK.Infrastructure.Database.Helpers;
using DSK.Infrastructure.Interfaces.DbHelpers;
using DSK.Infrastructure.Repositories;
using DSK.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DSK.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureInfrastructure(
        this IServiceCollection services
        , string connectionString)
    {

        AddMappings(services);
        AddRepositories(services);
        AddServices(services);
        AddHelpers(services);
        AddDbContextSettings(connectionString);

        return services;
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<ICreditRepository, CreditRepository>();
    }

    private static void AddMappings(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }

    private static void AddDbContextSettings(string connectionString)
    {
        DbSeedHelper.CreateDatabase(connectionString);
    }


    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ICreditService, CreditService>();
    }

    private static void AddHelpers(IServiceCollection services)
    {
        services.AddScoped<IDbHelper, DbHelper>();
    }
}