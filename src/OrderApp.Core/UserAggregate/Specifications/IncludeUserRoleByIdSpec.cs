namespace OrderApp.Core.UserAggregate.Specifications;

public class IncludeUserRoleByIdSpec:Specification<User>
{
    public IncludeUserRoleByIdSpec(int id)
    {
        Query
            .Include(u => u.Roles)
            .Where(u=> u.Id == id);
    }
}
