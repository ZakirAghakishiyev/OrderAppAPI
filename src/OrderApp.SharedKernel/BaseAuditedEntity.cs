namespace OrderApp.SharedKernel;

public abstract class BaseAuditedEntity<TUser> : BaseEntity, IAuditedEntity<TUser> where TUser : BaseUserEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int CreatedUserId { get; set; }
    public int? ModifiedUserId { get; set; }
    public TUser CreatedUser { get; set; } = null!;
    public TUser? ModifiedUser { get; set; }

    public void ClearAudit()
    {
        CreatedAt = default;
        CreatedUserId = default;
        ModifiedAt = null;
        ModifiedUser = null;
    }
}
