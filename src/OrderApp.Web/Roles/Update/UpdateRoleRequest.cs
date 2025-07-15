namespace OrderApp.Web.Roles.Update;

public class UpdateRoleRequest
{ 
  public const string Route = "/Roles/{Id:int}";
  public static string BuildRoute(int companyId) => Route.Replace("{Id:int}", companyId.ToString());

  public int Id { get; set; }

  public required string Name { get; set; }
}
