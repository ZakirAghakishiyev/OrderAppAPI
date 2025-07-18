using OrderApp.Core.UserAggregate;

namespace OrderApp.Web.Users;

public record UserRecord(int Id, string Name, string Email, List<RoleEnum> Roles);