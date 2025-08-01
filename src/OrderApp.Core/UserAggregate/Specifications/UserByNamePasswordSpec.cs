namespace OrderApp.Core.UserAggregate;
using OrderApp.Core.UserAggregate;

public class UserByNameAndPasswordSpec:Specification<User>
{
    public UserByNameAndPasswordSpec(string name, string password)
    {
        Query
            .Include(u => u.Roles)
            .ThenInclude(r=>r.Role)
            .Where(u => u.Name == name && u.Password == password);
    }
}
