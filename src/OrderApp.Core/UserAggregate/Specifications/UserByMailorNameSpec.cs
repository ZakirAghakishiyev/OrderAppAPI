namespace OrderApp.Core.UserAggregate.Specifications;

public class UserByMailorNameSpec:Specification<User>
{
    public UserByMailorNameSpec(string mail, string name)
    {
        Query.Where(u => u.Email == mail || u.Name == name);
    }
}
