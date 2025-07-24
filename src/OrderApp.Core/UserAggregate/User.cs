using OrderApp.SharedKernel;
using OrderApp.SharedKernel.Interfaces;
namespace OrderApp.Core.UserAggregate;

public class User : BaseUserEntity, IAggregateRoot
{
    public required string Name { get; set; } 
    public required string Password { get; set; } 
    public string Email { get; set; }= string.Empty;
    public List<UserRole> Roles { get; set; } = [];
}
