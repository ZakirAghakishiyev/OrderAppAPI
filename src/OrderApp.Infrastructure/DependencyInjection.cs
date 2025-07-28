using OrderApp.Core.Messaging;
using OrderApp.Infrastructure.Interceptors;
using OrderApp.Infrastructure.Messaging;

namespace OrderApp.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();
        services.AddScoped<EntityLoggingInterceptor>();
        return services;
    }
}

