using OrderApp.Core.UserAggregate;

namespace OrderApp.Web.Users.Update;

public class UpdateUserResponse(int id, string name, string email, string password, List<RoleEnum> roles)
{
  public int Id { get; set; } = id;
  public string Name { get; set; } = name;
  public string Email { get; set; } = email;
  public string Password { get; set; } = password;
  public required List<RoleEnum> Roles { get; set; } = roles;
}

