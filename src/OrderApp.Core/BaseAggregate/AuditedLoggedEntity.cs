using OrderApp.Core.UserAggregate;
using OrderApp.SharedKernel;

namespace OrderApp.Core.BaseAggregate;

public abstract class AuditedLoggedEntity : BaseAuditedLoggedEntity<User>
{
}
