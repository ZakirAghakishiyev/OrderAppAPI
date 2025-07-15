namespace OrderApp.Web.Products;

public class UpdateProductResponse(ProductRecord product)
{
    public ProductRecord Product { get; set; } = product;
}
