using OrderApp.Core.UserAggregate;

namespace OrderApp.Web.Users.List;

public class UserListResponse
{
    public List<UserRecord> Users { get; set; } = [];
}
