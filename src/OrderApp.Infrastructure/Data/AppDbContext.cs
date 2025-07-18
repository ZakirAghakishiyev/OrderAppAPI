using OrderApp.Core.ContributorAggregate;
using OrderApp.Core.UserAggregate;
using OrderApp.Core.CompanyAggregate;
using OrderApp.Core.ProductAggregate;
using OrderApp.Core.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Ardalis.SharedKernel;
using OrderApp.Infrastructure.Interceptors;
using OrderApp.SharedKernel;
using System.Linq.Expressions;

namespace OrderApp.Infrastructure.Data;
public class AppDbContext : DbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;
  private readonly AuditSaveChangesInterceptor _auditInterceptor;
  private readonly SoftDeleteInterceptor _softDeleteInterceptor;


    public AppDbContext(DbContextOptions<AppDbContext> options,
            AuditSaveChangesInterceptor auditInterceptor, SoftDeleteInterceptor softDeleteInterceptor, IDomainEventDispatcher? dispatcher = null)
          : base(options)
    {
        _dispatcher = dispatcher;
        _auditInterceptor = auditInterceptor;
        _softDeleteInterceptor = softDeleteInterceptor;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor);
        optionsBuilder.AddInterceptors(_softDeleteInterceptor);
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
    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
        if (typeof(ISoftDeletedEntity<User>).IsAssignableFrom(entityType.ClrType))
        {
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var isDeletedProp = Expression.Property(parameter, nameof(ISoftDeletedEntity<User>.IsDeleted));
            var condition = Expression.Equal(isDeletedProp, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
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
        .HasKey(ur => ur.Id); 

    modelBuilder.Entity<UserRole>()
        .Property(ur => ur.Id)
        .ValueGeneratedOnAdd(); 

    modelBuilder.Entity<UserRole>()
        .HasOne(ur => ur.User)
        .WithMany(u => u.Roles)
        .HasForeignKey(ur => ur.UserId)
        .OnDelete(DeleteBehavior.NoAction); 

    modelBuilder.Entity<UserRole>()
        .HasOne(ur => ur.Role)
        .WithMany()
        .HasForeignKey(ur => ur.RoleId)
        .OnDelete(DeleteBehavior.Restrict);
    modelBuilder.Entity<User>()
        .HasMany(u => u.Roles)
        .WithOne()
        .HasForeignKey(ur => ur.UserId)
        .OnDelete(DeleteBehavior.Cascade);


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
