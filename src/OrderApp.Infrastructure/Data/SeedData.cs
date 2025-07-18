using Microsoft.EntityFrameworkCore;
using OrderApp.Core.UserAggregate;
using OrderApp.Core.CompanyAggregate;
using OrderApp.Core.ProductAggregate;
using OrderApp.Core.OrderAggregate;

namespace OrderApp.Infrastructure.Data;

public static class SeedData
{
  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    if (await dbContext.Users.AnyAsync()) return; // DB has been seeded
    else await PopulateUserDataAsync(dbContext);
    if (await dbContext.Roles.AnyAsync()) return; // DB has been seeded
    else await PopulateRoleDataAsync(dbContext);
    if (await dbContext.Companies.AnyAsync()) return; // DB has been seeded
    else await PopulateCompanyDataAsync(dbContext);
    if (await dbContext.Products.AnyAsync()) return; // DB has been seeded
    else await PopulateProductDataAsync(dbContext);
    if (await dbContext.Orders.AnyAsync()) return; // DB has been seeded
    else await PopulateOrderDataAsync(dbContext);
    System.Console.WriteLine("Populating Order Data in initializer...");
  }

  public static async Task PopulateCompanyDataAsync(AppDbContext dbContext)
  {
    if (await dbContext.Companies.AnyAsync()) return; // DB has been seeded

    var company1 = new Company { Name = "Acme Corp" };
    var company2 = new Company { Name = "Tech Innovations" };

    dbContext.Companies.AddRange(company1, company2);
    await dbContext.SaveChangesAsync();
  }

  public static async Task PopulateProductDataAsync(AppDbContext dbContext)
  {
    if (await dbContext.Products.AnyAsync()) return;
    Product product1 = new() { Name = "Laptop", Price = 999.99m, CompanyId = 1 };
    Product product2 = new(){ Name = "Smartphone", Price = 499.99m, CompanyId = 2};

    dbContext.Products.AddRange(product1, product2);

    await dbContext.SaveChangesAsync();
  }

  public static async Task PopulateUserDataAsync(AppDbContext dbContext)
  {
    if (await dbContext.Users.AnyAsync()) return; 

    var user1 = new User { Name = "Alice", Password="", Email = "alice@gmail.com"};
    var user2 = new User { Name = "Bob", Password="", Email="bob@gmail.com"};

    dbContext.Users.AddRange(user1, user2);
    await dbContext.SaveChangesAsync();
  }

  public static async Task PopulateRoleDataAsync(AppDbContext dbContext)
  {
    if (await dbContext.Roles.AnyAsync()) return;

    var role1 = new Role { Name = "Admin" };
    var role2 = new Role { Name = "Customer" };

    dbContext.Roles.AddRange(role1, role2);
    await dbContext.SaveChangesAsync();
    
  }
  
  public static async Task PopulateOrderDataAsync(AppDbContext dbContext)
  {
    System.Console.WriteLine("Checking if Order Data needs to be populated...");
    if (await dbContext.Orders.AnyAsync()) return;
    System.Console.WriteLine("Populating Order Data...");
    var order1 = new Order { Id = 1, OrderDate = DateTime.UtcNow, UserId = 1, ProductId = 1 };
    var order2 = new Order { Id = 2, OrderDate = DateTime.UtcNow, UserId = 2, ProductId = 2 };

    dbContext.Orders.AddRange(order1, order2);
    await dbContext.SaveChangesAsync();
  }
}
