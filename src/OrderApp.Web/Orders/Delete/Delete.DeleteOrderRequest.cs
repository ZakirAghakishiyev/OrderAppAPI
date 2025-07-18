using Microsoft.AspNetCore.Mvc;

namespace OrderApp.Web.Orders.Delete;

public class DeleteOrderRequest
{
    
    public const string Route = "/Orders/{Id:int}";
    public static string BuildRoute(int id) => Route.Replace("{Id:int}", id.ToString());
    [FromRoute]
    public int Id { get; set; }
}
