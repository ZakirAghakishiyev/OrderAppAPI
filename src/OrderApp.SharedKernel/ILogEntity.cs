namespace OrderApp.SharedKernel;

public interface ILogEntity
{
    public int LogId { get; set; }
    public DateTime LogDateTime { get; set; }
}

public interface ILogEntity<TLogOperation, TUser> : ILogEntity
    where TLogOperation : BaseLogOperation where TUser : BaseUserEntity
{
    int LogOperationId { get; set; }
    int LogUserId { get; set; }
    TLogOperation LogOperation { get; set; }
    TUser LogUser { get; set; }
}
