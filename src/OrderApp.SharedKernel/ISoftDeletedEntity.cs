namespace OrderApp.SharedKernel;

public interface ISoftDeletedEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? DeletedUserId { get; set; }
    public int? RestoredUserId { get; set; }
    public DateTime? RestoredAt { get; set; }
}

public interface ISoftDeletedEntity<TUser> : ISoftDeletedEntity where TUser : BaseUserEntity
{

    public TUser? DeletedUser { get; set; }
    public TUser? RestoredUser { get; set; }

    public void SoftDelete(int userId)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedUser = null;
        DeletedUserId = userId;
    }

    public void RestoreSoftDelete(int userId)
    {
        ResetAll();
        RestoredUserId = userId;
        RestoredAt = DateTime.Now;
    }

    public void ResetAll()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedUserId = null;
        DeletedUser = null;
        RestoredUserId = null;
        RestoredUser = null;
        RestoredAt = null;
    }

    public void CheckIsDeleted()
    {
        if (IsDeleted == true)
        {
            if (DeletedAt == null)
            {
                throw new InvalidOperationException($"{nameof(DeletedAt)} cannot be null if entity is deleted");
            }

            if (DeletedUserId == null)
            {
                throw new InvalidOperationException($"{nameof(DeletedUserId)} cannot be null if entity is deleted");
            }
        }
    }
}
