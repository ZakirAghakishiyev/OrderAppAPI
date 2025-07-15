using Ardalis.Specification;
using MO=OrderApp.Core.OrderAggregate;

namespace OrderApp.Core.OrderAggregate.Specification;

public class OrderByIdWithIncludesSpec : Specification<MO.Order>
{
    public OrderByIdWithIncludesSpec(int id)
    {
        Query
            .Where(o => o.Id == id)
            .Include(o => o.Product)
            .ThenInclude(p=>p!.Company)
            .Include(o => o.User);
    }
}
