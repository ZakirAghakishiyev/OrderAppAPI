using Ardalis.Specification;
using MO=OrderApp.Core.OrderAggregate;

namespace OrderApp.Core.OrderAggregate.Specification;

public class OrdersWithIncludesSpec : Specification<MO.Order>
{
    public OrdersWithIncludesSpec()
    {
         Query
            .Include(o => o.Product)
            .Include(o => o.User);
    }   
}
