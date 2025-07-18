using Microsoft.EntityFrameworkCore.Diagnostics;
using OrderApp.Core.Services;
using OrderApp.Core.UserAggregate;
using OrderApp.SharedKernel;

namespace OrderApp.Infrastructure.Interceptors;

public class SoftDeleteInterceptor(IAuthenticatedUserAccessor _accessor) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var userId = _accessor.User.Id;

        foreach (var entry in context.ChangeTracker.Entries()
                     .Where(e => e.Entity is ISoftDeletedEntity<User>))
        {
            var entity = (ISoftDeletedEntity<User>)entry.Entity;

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                entity.DeletedUserId = userId;
                entity.RestoredAt = null;
                entity.RestoredUserId = null;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
