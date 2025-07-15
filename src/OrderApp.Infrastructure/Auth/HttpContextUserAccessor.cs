namespace OrderApp.Infrastructure.Auth;

using System.Security.Claims;
using OrderApp.Core.Services;
using Microsoft.AspNetCore.Http;


public class HttpContextUserAccessor : IAuthenticatedUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextUserAccessor(IHttpContextAccessor accessor)
    {
        _httpContextAccessor = accessor;
    }

    public AuthenticatedUser User
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
                return new AuthenticatedUser(); // Or throw

            return new AuthenticatedUser
            {
                Id = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                     throw new Exception("Missing NameIdentifier claim")),
                Username = user.Identity?.Name ?? string.Empty
            };
        }
    }
}
