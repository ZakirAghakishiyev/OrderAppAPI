namespace OrderApp.SharedKernel;

public interface IAuditedEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}

public interface IAuditedEntity<TUser> : IAuditedEntity where TUser : BaseUserEntity
{
    public int CreatedUserId { get; set; }
    public int? ModifiedUserId { get; set; }
    public TUser CreatedUser { get; set; }
    public TUser? ModifiedUser { get; set; }

    public void SetCreated(int userId)
    {
        CreatedUserId = userId;
        CreatedAt = DateTime.Now;
        ModifiedUserId = null;
        ModifiedAt = null;
    }

    public void SetModified(int userId)
    {
        ModifiedUserId = userId;
        ModifiedAt = DateTime.Now;
    }
}
