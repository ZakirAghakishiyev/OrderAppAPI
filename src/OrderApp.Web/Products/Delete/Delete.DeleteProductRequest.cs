namespace OrderApp.Web.Products;

public record DeleteProductRequest
{
    public const string Route = "/Products/{Id:int}";
    public static string BuildRoute(int Id) => Route.Replace("{Id:int}", Id.ToString());
    public int Id { get; set; }
}
