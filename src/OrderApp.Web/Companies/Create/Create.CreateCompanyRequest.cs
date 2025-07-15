namespace OrderApp.Web.Companies.Create;

public class CreateCompanyRequest
{
    public const string Route = "/Companies";
    public static string BuildRoute() => Route;

    public required string Name { get; set; } = string.Empty;
}
