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
using OrderApp.Core.BaseAggregate;

namespace OrderApp.Infrastructure.Data;
public class AppDbContext : DbContext
{
    private readonly IDomainEventDispatcher? _dispatcher;
    private readonly AuditSaveChangesInterceptor _auditInterceptor;
    private readonly SoftDeleteInterceptor _softDeleteInterceptor;
    private readonly EntityLoggingInterceptor _entityLoggingInterceptor;


    public AppDbContext(DbContextOptions<AppDbContext> options,
            AuditSaveChangesInterceptor auditInterceptor, SoftDeleteInterceptor softDeleteInterceptor, EntityLoggingInterceptor entityLoggingInterceptor, IDomainEventDispatcher? dispatcher = null)
          : base(options)
    {
        _dispatcher = dispatcher;
        _auditInterceptor = auditInterceptor;
        _softDeleteInterceptor = softDeleteInterceptor;
        _entityLoggingInterceptor = entityLoggingInterceptor;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor);
        optionsBuilder.AddInterceptors(_softDeleteInterceptor);
        optionsBuilder.AddInterceptors(_entityLoggingInterceptor);
    }
    public DbSet<Company> Companies { set; get; }
    public DbSet<Product> Products { set; get; }
    public DbSet<User> Users { set; get; }
    public DbSet<Role> Roles { set; get; }
    public DbSet<UserRole> UserRoles { set; get; }
    public DbSet<Order> Orders { set; get; }
    public DbSet<LoggedOrder> LoggedOrders { set; get; }
    public DbSet<LogAction> LogActions { set; get; }

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
        modelBuilder.Entity<LogAction>()
            .HasKey(la => la.Id);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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
        modelBuilder.Entity<LoggedOrder>()
            .HasOne(lo => lo.Product)
            .WithMany()
            .HasForeignKey(lo => lo.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<LoggedOrder>()
            .HasOne(lo => lo.Order)
            .WithMany()
            .HasForeignKey(lo => lo.OrderId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<LoggedOrder>()
            .HasOne(lo => lo.User)
            .WithMany()
            .HasForeignKey(lo => lo.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<LoggedOrder>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey("CreatedUserId")
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<LoggedOrder>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey("ModifiedUserId")
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<LoggedOrder>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey("DeletedUserId")
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
    }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    if (_dispatcher == null) return result;

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
