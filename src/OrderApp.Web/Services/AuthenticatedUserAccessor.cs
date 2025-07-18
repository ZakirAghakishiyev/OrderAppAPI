namespace OrderApp.Web.Services;

using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using OrderApp.Core.Services;
using OrderApp.Core.UserAggregate;


public class AuthenticatedUserAccessor : IAuthenticatedUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticatedUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public AuthenticatedUser User
{
    get
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || user.Identity?.IsAuthenticated != true)
            return new AuthenticatedUser { Id = 0, Username = "Anonymous" };

        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = user.Identity.Name ?? "";

        if (int.TryParse(idClaim, out var id))
        {
            return new AuthenticatedUser { Id = id, Username = username, UserRoles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => new UserRole { RoleId = (int)Enum.Parse<RoleEnum>(c.Value) })
                .ToList() };
        }

        return new AuthenticatedUser { Id = 0, Username = username };
    }
}

}

