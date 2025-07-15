namespace OrderApp.Core.UserAggregate.Specification;

using OrderApp.Core.UserAggregate;

public class RoleByNameSpec : Specification<Role>
{
    public RoleByNameSpec(string roleName)
    {
        Query.Where(r => r.Name == roleName);
    }
}
