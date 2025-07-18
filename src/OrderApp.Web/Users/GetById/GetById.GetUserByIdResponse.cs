using OrderApp.Core.UserAggregate;

namespace OrderApp.Web.Users.GetById;

public class GetUserByIdResponse
{
    public int Id { set; get; }
    public string? Name { set; get; }
    public string? Email { set; get; }
    public required List<RoleEnum> Roles { get; set; } = [];
}
