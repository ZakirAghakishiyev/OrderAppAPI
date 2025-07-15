using OrderApp.Web.Companies.GetById;

namespace OrderApp.Web.Products.GetById;

public class GetProductByIdResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CompanyId { get; set; }
    public GetCompanyByIdResponse? Company { get; set; }
}
