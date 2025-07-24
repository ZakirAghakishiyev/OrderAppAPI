namespace OrderApp.Core.BaseAggregate;

public interface ILoggedEntity
{
    public int OrderId{ get; set; }
    public int LogActionId { get; set; }
}
