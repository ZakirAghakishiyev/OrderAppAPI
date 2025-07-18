using OrderApp.Core.BaseAggregate;
using OrderApp.SharedKernel;
using OrderApp.SharedKernel.Interfaces;

namespace OrderApp.Core.UserAggregate;

public class Role : BaseEntity, INamedEntity, IAggregateRoot
{
    public required string Name { get; set; }
}

public enum RoleEnum
{
    Admin = 1,
    Customer = 2
}
