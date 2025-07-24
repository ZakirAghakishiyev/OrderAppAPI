using OrderApp.Infrastructure.Interceptors;

namespace OrderApp.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();
        services.AddScoped<EntityLoggingInterceptor>();
        return services;
    }
}

