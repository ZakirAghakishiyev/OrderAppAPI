namespace OrderApp.Web.Products;

public class CreateProductRequest
{
    public const string Route = "/Products";
    public static string BuildRoute() => Route;
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int CompanyId { get; set; }
}