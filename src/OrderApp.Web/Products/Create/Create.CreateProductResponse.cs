namespace OrderApp.Web.Products;

public class CreateProductResponse(int id, string name, decimal price, int companyId)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
    public int CompanyId { get; set; } = companyId;
}
