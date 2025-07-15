namespace OrderApp.Web.Products.GetById;

public class GetProductByIdRequest
{
    public const string Route = "/Products/{id}";
    public static string BuildRoute(int id) => Route.Replace("{id}", id.ToString());

    public required int Id { get; set; }
}
