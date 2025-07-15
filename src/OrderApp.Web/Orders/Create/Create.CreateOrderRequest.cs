namespace OrderApp.Web.Orders.Create;

public class CreateOrderRequest
{
    public const string Route = "/Orders";
    public static string BuildRoute() => Route;
    public required DateTime OrderDate { get; set; }
    public required int UserId { get; set; }
    public required int ProductId { get; set; }
}
