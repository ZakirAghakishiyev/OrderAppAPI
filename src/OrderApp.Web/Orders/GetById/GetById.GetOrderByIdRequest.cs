namespace OrderApp.Web.Orders.GetById;

public class GetOrderByIdRequest
{
    public const string Route = "/Orders/{id}";
    public static string BuildRoute(int id) => Route.Replace("{id}", id.ToString());

    public required int Id { get; set; }
}

