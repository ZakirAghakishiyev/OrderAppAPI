using OrderApp.Core.OrderAggregate;

namespace OrderApp.Web.Orders.OrderLogs;

public class GetOrderLogsResponse
{
    public List<LoggedOrder> OrderLogs{ get; set; } = new();
}
