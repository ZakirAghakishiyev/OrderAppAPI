namespace OrderApp.Core.UserAggregate;
using OrderApp.Core.UserAggregate;

public class UserByNameAndPasswordSpec:Specification<User>
{
    public UserByNameAndPasswordSpec(string name, string password)
    {
        Query.Where(u => u.Name == name && u.Password == password);
    }
}
