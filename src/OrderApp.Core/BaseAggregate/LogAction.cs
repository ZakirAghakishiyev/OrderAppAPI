namespace OrderApp.Core.BaseAggregate;

public class LogAction
{
    public int Id { get; set; }
    public required string ActionType { get; set; }
}
