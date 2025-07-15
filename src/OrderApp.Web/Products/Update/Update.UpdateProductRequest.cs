using Microsoft.AspNetCore.Mvc;

namespace OrderApp.Web.Products;

public class UpdateProductRequest
{
    public const string Route = "/Products/{Id:int}";
    public static string BuildRoute(int id) => Route.Replace("{Id:int}", id.ToString());
    [FromRoute]
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int CompanyId { get; set; }
}


