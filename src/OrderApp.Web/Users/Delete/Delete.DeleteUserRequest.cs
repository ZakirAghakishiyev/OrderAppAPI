namespace OrderApp.Web.Users.Delete;

public class DeleteUserRequest
{
    public const string Route = "/Users/{UserId:int}";
    public static string BuildRoute(int userId) => Route.Replace("{UserId:int}", userId.ToString());
    
    public int UserId { get; set; }
}
