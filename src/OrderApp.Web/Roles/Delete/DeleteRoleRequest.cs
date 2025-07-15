namespace OrderApp.Web.Roles.Delete;

public class DeleteRoleRequest
{
    
  public const string Route = "/Roles/{Id:int}";
    public static string BuildRoute(int companyId) => Route.Replace("{Id:int}", companyId.ToString());

  public int Id { get; set; }
}
