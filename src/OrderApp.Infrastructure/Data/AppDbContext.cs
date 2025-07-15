using OrderApp.Core.ContributorAggregate;
using OrderApp.Core.UserAggregate;
using OrderApp.Core.CompanyAggregate;
using OrderApp.Core.ProductAggregate;
using OrderApp.Core.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Ardalis.SharedKernel;
using OrderApp.Infrastructure.Interceptors;

namespace OrderApp.Infrastructure.Data;
public class AppDbContext : DbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;
  private readonly AuditSaveChangesInterceptor _auditInterceptor;

  public AppDbContext(DbContextOptions<AppDbContext> options,
        AuditSaveChangesInterceptor auditInterceptor, IDomainEventDispatcher? dispatcher = null)
      : base(options)
  {
    _dispatcher = dispatcher;
    _auditInterceptor = auditInterceptor;
  }
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor);
    }
  // public DbSet<Contributor> Contributors => Set<Contributor>();
  public DbSet<Company> Companies { set; get; }
  public DbSet<Product> Products { set; get; }
  public DbSet<User> Users { set; get; }
  public DbSet<Role> Roles { set; get; }
  public DbSet<UserRole> UserRoles { set; get; }
  public DbSet<Order> Orders { set; get; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Order>()
        .HasOne(o => o.User)
        .WithMany()
        .HasForeignKey(o => o.UserId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Order>()
        .HasOne(o => o.DeletedUser)
        .WithMany()
        .HasForeignKey("DeletedUserId")
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Order>()
        .HasOne(o => o.RestoredUser)
        .WithMany()
        .HasForeignKey("RestoredUserId")
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Order>()
        .HasOne(o => o.CreatedUser)
        .WithMany()
        .HasForeignKey("CreatedUserId")
        .OnDelete(DeleteBehavior.NoAction);
    modelBuilder.Entity<UserRole>()
        .HasOne(u=>u.User)
        .WithMany()
        .HasForeignKey("UserId")
        .OnDelete(DeleteBehavior.NoAction);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<HasDomainEventsBase>()
        .Select(e => e.Entity)
        .Where(e => e.DomainEvents.Any())
        .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();
}
