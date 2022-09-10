using FC.Codeflix.Catalog.Infra.DataEF;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Api.Configurations;

public static class ConnectionsConfiguration
{
    public static IServiceCollection AddConnectionsConfig(this IServiceCollection services)
    {
        services.AddDbConnection();
        return services;
    }
    private static IServiceCollection AddDbConnection(this IServiceCollection services)
    {
        services.AddDbContext<CodeflixCatalogDbContext>(
            options => options.UseInMemoryDatabase(
                "inMemory-DSV-Database"
                )
            );
        return services;
    }
}
