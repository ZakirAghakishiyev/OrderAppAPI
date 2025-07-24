namespace OrderApp.Core.OrderAggregate.Specifications;

public class OrderLogsSpec:Specification<LoggedOrder>
{
    public OrderLogsSpec()
    {
        Query
            .Include(o => o.Order)
            .ThenInclude(o => o!.Product)
            .ThenInclude(p => p!.Company)
            .Include(o => o.Order!.User);
    }
}
