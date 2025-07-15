namespace OrderApp.SharedKernel;

public abstract class BaseAuditedLoggedEntity<TUser> : BaseAuditedEntity<TUser>, ILoggedEntity
    where TUser : BaseUserEntity
{
}
