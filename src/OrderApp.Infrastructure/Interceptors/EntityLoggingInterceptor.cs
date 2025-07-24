using System.Text.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OrderApp.Core.BaseAggregate;
using OrderApp.Core.OrderAggregate;
using OrderApp.Core.Services;

namespace OrderApp.Infrastructure.Interceptors;
public class AuditLog
{
    public int Id { get; set; }
    public string EntityName { get; set; } = default!;
    public string Action { get; set; } = default!;
    public string KeyValues { get; set; } = default!;
    public string OldValues { get; set; } = default!;
    public string NewValues { get; set; } = default!;
    public int? UserId { get; set; }
    public DateTime Timestamp { get; set; }
}

public class EntityLoggingInterceptor(IAuthenticatedUserAccessor _accessor) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);
        var userId = _accessor.User.Id;
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is ILoggedEntity)
            .ToList();

        foreach (var entry in entries)
        {
            var entityType = entry.Entity.GetType();
            var keyValues = new Dictionary<string, object?>();
            foreach (var prop in entityType.GetProperties().Where(p => p.Name.EndsWith("Id")))
                keyValues[prop.Name] = entry.Property(prop.Name).CurrentValue;

            var oldValues = new Dictionary<string, object?>();
            var newValues = new Dictionary<string, object?>();
            foreach (var prop in entityType.GetProperties())
            {
                oldValues[prop.Name] = entry.State == EntityState.Added ? null : entry.OriginalValues[prop.Name];
                newValues[prop.Name] = entry.CurrentValues[prop.Name];
            }

            var log = new AuditLog
            {
                EntityName = entityType.Name,
                Action = entry.State.ToString(),
                KeyValues = JsonSerializer.Serialize(keyValues),
                OldValues = JsonSerializer.Serialize(oldValues),
                NewValues = JsonSerializer.Serialize(newValues),
                UserId = userId,
                Timestamp = DateTime.UtcNow
            };

            context.Set<AuditLog>().Add(log);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}