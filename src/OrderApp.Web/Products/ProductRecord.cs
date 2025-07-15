using OrderApp.Core.CompanyAggregate;
using OrderApp.Core.ProductAggregate;

namespace OrderApp.Web.Products;

public class ProductRecord(int id, string name, decimal price, int companyId, Company? company = null)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
    public int CompanyId { get; set; } = companyId;
    public Company? Company { get; set; } = company;
}
