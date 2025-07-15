namespace OrderApp.Web.Companies;

public class UpdateCompanyRequest
{
  public const string Route = "/Companies/{CompanyId:int}";
  public static string BuildRoute(int companyId) => Route.Replace("{CompanyId:int}", companyId.ToString());

  public int CompanyId { get; set; }

  public required string Name { get; set; }
}
