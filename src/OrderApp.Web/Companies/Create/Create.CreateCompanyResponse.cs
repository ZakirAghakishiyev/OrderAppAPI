namespace OrderApp.Web.Companies.Create;

public class CreateCompanyResponse(int id, string name)
{
    public int Id { set; get; } = id;
    public string Name { set; get; } = name;
}
