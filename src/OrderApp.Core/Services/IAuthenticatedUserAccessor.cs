using OrderApp.Core.UserAggregate;

namespace OrderApp.Core.Services;

public interface IAuthenticatedUserAccessor
{
    AuthenticatedUser User { get; }
}

public class AuthenticatedUser
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

