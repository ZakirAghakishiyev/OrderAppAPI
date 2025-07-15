using OrderApp.Core.UserAggregate;
using OrderApp.SharedKernel;
namespace OrderApp.Core.BaseAggregate;

public abstract class AuditedEntity : BaseAuditedEntity<User>
{
}
