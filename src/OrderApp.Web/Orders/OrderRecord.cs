using OrderApp.Core.ProductAggregate;
using OrderApp.Core.UserAggregate;

namespace OrderApp.Web.Orders;

public class OrderRecord
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public User? User { get; set; }

    public OrderRecord() { }

    public OrderRecord(int id, DateTime orderDate, int userId, int productId, Product? product = null, User? user = null)
    {
        Id = id;
        OrderDate = orderDate;
        UserId = userId;
        ProductId = productId;
        Product = product;
        User = user;
    }
}
