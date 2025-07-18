namespace OrderApp.Core.UserAggregate.Specifications;

public class IncludeUserRolesSpec:Specification<User>
{
    public IncludeUserRolesSpec()
    {
        Query
            .Include(u => u.Roles);
    }
}
