namespace OrderApp.Web.Roles.Create;

public class CreateRoleRequest
{
    public const string Route = "/Roles";
    public static string BuildRoute() => Route;

    public required string Name { get; set; } = string.Empty;
}
