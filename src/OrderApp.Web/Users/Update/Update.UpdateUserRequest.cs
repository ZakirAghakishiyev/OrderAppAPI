using System.ComponentModel.DataAnnotations;

namespace OrderApp.Web.Users.Update;

public class UpdateUserRequest
{
    public const string Route = "/Users/{UserId:int}";
    public static string BuildRoute(int userId) => Route.Replace("{UserId:int}", userId.ToString());

    public int UserId { get; set; }

    public required string Name { get; set; } = string.Empty;
    [EmailAddress]
    public required string Email { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public required List<int> Roles { get; set; } = [];
}
