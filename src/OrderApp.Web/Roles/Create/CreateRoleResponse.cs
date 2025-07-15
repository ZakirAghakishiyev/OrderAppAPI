namespace OrderApp.Web.Roles.Create;

public class CreateRoleResponse(int id, string name)
{
    public int Id { set; get; } = id;
    public string Name { set; get; } = name;
}
