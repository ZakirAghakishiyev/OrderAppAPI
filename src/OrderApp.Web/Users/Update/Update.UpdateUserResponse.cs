using OrderApp.Core.UserAggregate;

namespace OrderApp.Web.Users.Update;

public class UpdateUserResponse(int id, string name,string password)
{
  public int Id { get; set; } = id;
  public string Name { get; set; } = name;
  public string Password { get; set; } = password;
}

