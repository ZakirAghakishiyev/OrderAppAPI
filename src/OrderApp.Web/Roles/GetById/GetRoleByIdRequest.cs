namespace OrderApp.Web.Roles.GetById;

public class GetRoleByIdRequest
{
  public const string Route = "/Roles/{Id:int}";
  public static string BuildRoute(int companyId) => Route.Replace("{Id:int}", companyId.ToString());
  public int Id{ get; set; }
}
