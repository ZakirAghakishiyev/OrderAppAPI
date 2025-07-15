namespace OrderApp.Web.Orders.List;

public class OrderListResponse
{
    public List<OrderRecord> Orders { get; set; } = new();
}
