using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Users.List;

public class List(IUserEndpointService _userEndpointService) : EndpointWithoutRequest<UserListResponse>
{
    public override void Configure()
    {
        Get("/Users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var users = await _userEndpointService.ListAsync(cancellationToken);
        Response = users;
    }
}
