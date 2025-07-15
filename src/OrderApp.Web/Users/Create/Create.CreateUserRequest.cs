using System.ComponentModel.DataAnnotations;

namespace OrderApp.Web.Users.Create;

public class CreateUserRequest
{
  public const string Route = "/Users";

  public required string Name { get; set; } = string.Empty;
  public required string Password { get; set; } = string.Empty;
}
