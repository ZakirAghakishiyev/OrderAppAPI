using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OrderApp.Core.Services;
using OrderApp.Core.UserAggregate;
using OrderApp.SharedKernel;
using OrderApp.SharedKernel.Interfaces;

namespace OrderApp.Infrastructure.Interceptors;

public class AuditSaveChangesInterceptor(ILogger<AuditSaveChangesInterceptor> _logger,IAuthenticatedUserAccessor _accessor) : SaveChangesInterceptor
{

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var entry in context.ChangeTracker.Entries<IAuditedEntity<User>>())
        {
            if (entry.State == EntityState.Added)
            {
                _logger.LogInformation("Audit interceptor: setting CreatedAt and CreatedUserId for entity {EntityType}", entry.Entity.GetType().Name);
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedUserId = _accessor.User.Id;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
