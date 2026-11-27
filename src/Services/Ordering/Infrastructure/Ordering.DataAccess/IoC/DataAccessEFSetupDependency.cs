using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.DataAccess.Interceptors;

namespace Ordering.DataAccess.Ef;

public static class DataAccessEFSetupDependency
{
    public static IServiceCollection AddOrderingDataAccessEf(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderingDbContext>((sp, options) =>
        {
            var connectionString = configuration.GetConnectionString("OrderingDatabase");
            var outboxInterceptor = sp.GetRequiredService<OutboxDomainEventInterceptor>();
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "Ordering");
                npgsql.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            }).AddInterceptors(outboxInterceptor);
        });
        return services;
    }
}