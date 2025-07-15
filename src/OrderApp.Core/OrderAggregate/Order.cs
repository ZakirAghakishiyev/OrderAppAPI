namespace OrderApp.Core.OrderAggregate;

using OrderApp.Core.UserAggregate;
using OrderApp.Core.ProductAggregate;
using OrderApp.Core.BaseAggregate;
using OrderApp.SharedKernel.Interfaces;

public class Order : AuditedSoftDeletedEntity,IAggregateRoot
{
    public DateTime OrderDate { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
}
