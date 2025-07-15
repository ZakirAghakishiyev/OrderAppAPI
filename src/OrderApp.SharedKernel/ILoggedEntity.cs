namespace OrderApp.SharedKernel;

public interface ILoggedEntity
{
    public const string DEFAULT_LOG_ENTITY_SUFFIX = "_Logs";
    public Type? GetLogEnityType() => null;

    public string? GetLogEnityTypeAndAssemblyFullName() => null;

    public string? GetLogEnityTypeSuffix() => "Log";
}
