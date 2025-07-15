namespace OrderApp.Core.UserAggregate;

public class UserRole
{
    public int Id { set; get; }
    public int UserId { set; get; }
    public User? User{ set; get; }
    public int RoleId { set; get; }
    public Role? Role{ set; get; }
}
