using Microsoft.AspNetCore.Authorization;

using OrderApp.Core;
using OrderApp.Endpoint.Attributes;
using OrderApp.Core.Services;
using OrderApp.Infrastructure.Auth;

namespace OrderApp.Web.Security.Authorization;

public class RoutePermissionAuthorizationHandler(IHttpContextAccessor httpContextAccessor, IAuthenticatedUserAccessor authenticatedUserAccessor) : AuthorizationHandler<RoutePermissionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor = authenticatedUserAccessor;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoutePermissionRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var endpoint = httpContext.GetEndpoint();
        if (endpoint == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var permissionAttribute = endpoint.Metadata.GetMetadata<PermissionAttribute>();
        if (permissionAttribute == null || permissionAttribute.Permissions.Length == 0)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var requiredRoles = permissionAttribute.Permissions.Select(p => (int)p);
        var userRoleIds = _authenticatedUserAccessor.User?.UserRoles.Select(ur => ur.RoleId).ToList();

        if (userRoleIds == null || userRoleIds.Count == 0)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        bool hasPermission = userRoleIds.Any(roleId => requiredRoles.Contains(roleId));

        if (hasPermission)
            context.Succeed(requirement);
        else
            context.Fail();

        return Task.CompletedTask;
    }
}