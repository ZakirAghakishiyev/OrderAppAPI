namespace OrderApp.Web.Users.Create;


public class CreateUserResponse(int id, string name,string password)
{
  public int Id { get; set; } = id;
  public string Name { get; set; } = name;
  public string Password { get; set; } = password;
}
