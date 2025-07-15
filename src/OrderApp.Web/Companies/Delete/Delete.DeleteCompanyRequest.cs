namespace OrderApp.Web.Companies.Delete;

public record DeleteCompanyRequest
{
  public const string Route = "/Companies/{CompanyId:int}";
  public static string BuildRoute(int companyId) => Route.Replace("{CompanyId:int}", companyId.ToString());

  public int CompanyId { get; set; }
}
