using Microsoft.AspNetCore.Mvc;

namespace OrderApp.Web.Orders.Update;

public class UpdateOrderRequest
{
    public const string Route = "/Orders/{Id:int}";
    public static string BuildRoute(int id) => Route.Replace("{Id:int}", id.ToString());


    public int Id { get; set; }
    public required DateTime OrderDate { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
}
