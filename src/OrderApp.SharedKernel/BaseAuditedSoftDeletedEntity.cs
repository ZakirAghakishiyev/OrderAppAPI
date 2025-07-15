namespace OrderApp.SharedKernel;

public abstract class BaseAuditedSoftDeletedEntity<TUser> : BaseAuditedEntity<TUser>, ISoftDeletedEntity<TUser>
    where TUser : BaseUserEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? DeletedUserId { get; set; }
    public TUser? DeletedUser { get; set; }
    public int? RestoredUserId { get; set; }
    public DateTime? RestoredAt { get; set; }
    public TUser? RestoredUser { get; set; }
}
