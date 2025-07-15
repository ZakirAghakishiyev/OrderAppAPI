

using OrderApp.Core.UserAggregate;

namespace OrderApp.Endpoint.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class PermissionAttribute(params RoleEnum[] permissions) : Attribute
{
    public RoleEnum[] Permissions { get; } = permissions;
}