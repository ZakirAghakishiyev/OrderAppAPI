using Microsoft.AspNetCore.Http.HttpResults;
using OrderApp.Infrastructure.Data;

namespace OrderApp.Web.Users.GetById;

public class GetById(IUserEndpointService _userEndpointService):Endpoint<GetUserByIdRequest, GetUserByIdResponse>
{
    public override void Configure()
    {
        Get(GetUserByIdRequest.Route);
        AllowAnonymous();
    }

    public override async Task<GetUserByIdResponse> HandleAsync(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userEndpointService.GetByIdAsync(request,cancellationToken);
        if (user == null)
        {
            await SendNotFoundAsync(cancellationToken);
            return null!;
        }

        Response = user;
        return Response;
    }
}
