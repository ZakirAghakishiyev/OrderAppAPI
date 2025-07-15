using OrderApp.Core.UserAggregate;
using OrderApp.Endpoint.Attributes;

namespace OrderApp.Web.Roles.List;

public class List(IRoleEndpointService _endpointService) : EndpointWithoutRequest<RoleListResponse>
{
    public override void Configure()
    {
        Get("/Roles");
        Policies(Endpoint.Constants.Policies.RoutePermissionPolicy);
        Options(opt => opt
            .WithMetadata(new PermissionAttribute(RoleEnum.Admin))
        );
    }

    public override async Task<RoleListResponse> HandleAsync(CancellationToken cancellationToken)
    {
        var response = await _endpointService.ListAsync(cancellationToken);
        if (response == null)
        {
            throw new InvalidOperationException("No roles found");
        }
        await SendAsync(response);
        return response;
    }
}
